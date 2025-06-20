using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Attributes.Injection;
using WillFrameworkPro.Core.CommandManager;
using WillFrameworkPro.Core.Containers;
using WillFrameworkPro.Core.Initialize;
using WillFrameworkPro.Core.Rules;
using WillFrameworkPro.Core.Views;
using WillFrameworkPro.StateMachine;

namespace WillFrameworkPro.Core.Context
{
    public class BaseContext<T> : IContext where T : BaseContext<T>
    {
        //防止重复启动
        private bool _hasStarted = false; 
        // --- IOC 容器 ---
        private readonly IocContainer _iocContainer = new();
        public IocContainer IocContainer { get => _iocContainer;}
        // --- Command 容器 ---
        private readonly CommandContainer _commandContainer = new();
        public CommandContainer CommandContainer { get => _commandContainer; }
        // --- State Machine 容器 ---
        private readonly StateContainer _stateContainer = new();
        public StateContainer StateContainer { get => _stateContainer; }
        public void PresetGeneratedView(IView view)
        {
            view.SetContext(Instance);
            IocContainer.Add(TypeEnum.View, view);
            PermissionFlags permissions = PermissionForIdentities.GetPermissionsByIdentityType(TypeEnum.View);
            InjectByPermission(view, permissions);
            //处理事件绑定与事件缓存
            HandleCommandListener(view);
            HandleAutoInitialize(view);
        }

        #region 获取的实例在任何情况下都是单例的
        private static readonly Lazy<T> _lazyCreateInstance = new(() =>
        {
            Type type = typeof(T);
            var constructorInfos = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            var constructorInfo = Array.Find(constructorInfos, c => c.GetParameters().Length == 0);
            if (constructorInfo == null)
            {
                throw new Exception("这个类没有找到无参且私有的构造器: " + typeof(T).FullName);
            }
            return constructorInfo.Invoke(null) as T;
        });

        public static T Instance
        {
            get => _lazyCreateInstance.Value;
        }
        #endregion
        
        #region 扫描类上面的特性, 添加进相应的 container
        private void ScanTypeAttributesByAssembly(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            foreach (var t in types)
            {
                BaseAttribute attribute = t.GetCustomAttribute(typeof(BaseAttribute)) as BaseAttribute;
                if (attribute == null)
                {
                    continue;
                }
                object instance = CreateInstance(t);
                HandleCanSetContext(instance);
                _iocContainer.Add(attribute.TypeEnum, instance);
            }
        }

        private void HandleCanSetContext(object instance)
        {
            ICanSetContext canSetContext = instance as ICanSetContext;
            if (canSetContext != null)
            {
                canSetContext.SetContext(this); 
            }
        }
        private object CreateInstance(Type type)
        {
            object instance;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (MissingMethodException e)
            {
                try
                {
                    instance = Activator.CreateInstance(type, true);
                }
                catch (MissingMethodException e2)
                {
                    Debug.LogError($"{type.FullName} 找不到无参构造函数");
                    throw e2;
                }
            }
            return instance;
        }
        #endregion

        private void HandleIdentities()
        {
            Dictionary<TypeEnum, Dictionary<Type, List<object>>> identityIoc  = IocContainer.IdentityIoc;
            foreach (KeyValuePair<TypeEnum, Dictionary<Type, List<object>>> outerKv in identityIoc)
            {
                TypeEnum typeEnum = outerKv.Key;

                foreach (KeyValuePair<Type,  List<object>> innerKv in outerKv.Value)
                {
                    List<object> objectList = innerKv.Value;
                    foreach (var instance in objectList)
                    {
                        PermissionFlags permissions = PermissionForIdentities.GetPermissionsByIdentityType(typeEnum);
                        //----- 依据权限注入受框架托管的引用
                        InjectByPermission(instance, permissions);
                        //----- 找到标注着 command listener 的方法，放进 command container 中托管
                        HandleCommandListener(instance);
                        //----- 构建 stateMachine 与 State 集合的映射关系，存进 State 容器中，方便后面注销 State 对象上绑定的事件。
                        HandleStateMachine(instance);
                        //----- 若重写了初始化方法，就执行初始化代码
                        HandleAutoInitialize(instance);
                    }
                }
                
            }
            
        }
        /// <summary>
        /// 递归查找所有（包括父类）的 listener 方法。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private List<MethodInfo> FindAllListenerMethods(Type type)
        {
            var result = new List<MethodInfo>();
            while (type != null && type != typeof(object))
            {
                //只查当前类型声明的实例方法
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                foreach (var m in methods)
                {
                    if (m.IsDefined(typeof(ListenerAttribute), false)) // false：只查当前方法声明的 Attribute
                    {
                        result.Add(m);
                    }
                }
                type = type.BaseType;
            }
            return result;
        }
        private void HandleCommandListener(object instance)
        {
            var methodsWithAttribute = FindAllListenerMethods(instance.GetType());
            foreach (var m in methodsWithAttribute)
            {
                var parameters = m.GetParameters();
                // 确保方法具有一个参数
                if (parameters.Length != 1)
                {
                    Debug.LogError(instance.GetType().FullName + " 类标注有 [Listener] 特性的方法只允许有一个参数。");
                    continue;
                }
                var commandType = parameters[0].ParameterType;
                // 确保参数类型实现了 ICommand 接口
                if (!typeof(ICommand).IsAssignableFrom(commandType))
                {
                    Debug.LogError(instance.GetType().FullName + " 类标注有 [Listener] 特性的方法参数类型必须要实现 ICommand 接口。");
                    continue;
                }
                // 构造泛型委托类型 InvokeCommandDelegate<T>
                var delegateType = typeof(CommandContainer.InvokeCommandDelegate<>).MakeGenericType(commandType);
                try
                {
                    // 创建委托实例
                    var del = Delegate.CreateDelegate(delegateType, instance, m);
                    // 获取 AddCommandListener<T> 方法
                    var addMethod = typeof(CommandContainer).GetMethod(nameof(CommandContainer.AddCommandListener)).MakeGenericMethod(commandType);
                    // 调用 AddCommandListener<T>(userInstance, del)
                    addMethod.Invoke(CommandContainer, new object[] { instance, del });
                }
                catch (Exception ex)
                {
                    // 处理委托创建或方法调用中的异常
                    Debug.LogError($"Context 反射注册 Command 方法 {m.Name} 时发生错误: {ex.Message}");
                }
            }
        }
        /// <summary>
        /// 构建 State Machine 与 State 集合的映射容器。
        /// </summary>
        /// <param name="instance"></param>
        private void HandleStateMachine(object instance)
        {
            if (instance is BaseStateMachine)
            {
                Type stateMachine = instance.GetType();
                // 查找所有公共和非公共的字段（非静态，且不包括继承的）
                FieldInfo[] fields = instance.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    //若字段上面标注有 Inject Attribute
                    if (field.IsDefined(typeof(InjectAttribute), false))
                    {
                        object fieldValue = field.GetValue(instance);
                        if (fieldValue is BaseState)
                        {
                            BaseState state = (BaseState)fieldValue;
                            //添加映射到 State 容器
                            StateContainer.AddState(stateMachine, state);
                        }
                    }
                }
            }
        }

        private void HandleAutoInitialize(object instance)
        {
            if (instance is IAutoInitialize auto)
            {
                auto.Initialize();
            }
        }
        private TypeEnum GetIdentityTypeByType(Type type)
        {
            foreach(KeyValuePair<TypeEnum, Dictionary<Type, List<object>>> kv in IocContainer.IdentityIoc)
            {
                if (kv.Value.TryGetValue(type, out List<object> instanceList))
                {
                    return kv.Key;
                }
            }
            return TypeEnum._None;
        }

        private void SetInstanceField(TypeEnum fieldTypeEnum, Type fieldType, object instance, FieldInfo f)
        {
            if (IocContainer.IdentityIoc.TryGetValue(fieldTypeEnum, out Dictionary<Type, List<object>> dic))
            {
                if (dic.TryGetValue(fieldType, out List<object> fieldInstanceList))
                {
                    f.SetValue(instance, fieldInstanceList.First());
                }
            }
        }

        private void ValidatePermissions(PermissionFlags permissions, PermissionFlags canInject, Type instanceType, TypeEnum injectTypeEnum)
        {
            if (!permissions.HasFlag(canInject))
            {
                throw new Exception($"{instanceType.FullName} 不允许注入 {nameof(injectTypeEnum)} 类型字段");
            }
        }
        
        private void InjectByPermission(object instance, PermissionFlags permissions)
        {
            Type type = instance.GetType();
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.NonPublic |
                                                    BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo f in fieldInfos)
            {
                if (f.GetCustomAttribute(typeof(InjectAttribute)) is InjectAttribute injectAttr)
                {
                    Type fieldType = f.FieldType;
                    TypeEnum fieldTypeEnum = GetIdentityTypeByType(fieldType);
                    if (fieldTypeEnum == TypeEnum._None)
                    {
                        throw new Exception($"{type.FullName} 无法注入非托管类型: {fieldType.FullName}");
                    }
                    if (fieldTypeEnum == TypeEnum.Model)
                    {
                        ValidatePermissions(permissions, PermissionFlags.Model, type, fieldTypeEnum);
                        SetInstanceField(fieldTypeEnum, fieldType, instance, f);
                        continue;
                    }
                    if (fieldTypeEnum == TypeEnum.Service)
                    {
                        ValidatePermissions(permissions, PermissionFlags.Service, type, fieldTypeEnum);
                        SetInstanceField(fieldTypeEnum, fieldType, instance, f);
                        continue;
                    }
                    if (fieldTypeEnum == TypeEnum.View)
                    {
                        ValidatePermissions(permissions, PermissionFlags.View, type, fieldTypeEnum);
                        SetInstanceField(fieldTypeEnum, fieldType, instance, f);
                        continue;
                    }

                    if (fieldTypeEnum == TypeEnum.General)
                    {
                        if (fieldType == typeof(LowLevelCommandManager) && !permissions.HasFlag(PermissionFlags.LowLevelCommandManager))
                        {
                            throw new Exception($"{type.FullName} 不允许注入 {nameof(LowLevelCommandManager)} 类型字段");
                        }
                        if (fieldType == typeof(HighLevelCommandManager) && !permissions.HasFlag(PermissionFlags.HighLevelCommandManager))
                        {
                            throw new Exception($"{type.FullName} 不允许注入 {nameof(HighLevelCommandManager)} 类型字段");
                        }
                        if (fieldType == typeof(CommandManager.CommandManager) && !permissions.HasFlag(PermissionFlags.CommandManager))
                        {
                            throw new Exception($"{type.FullName} 不允许注入 {nameof(CommandManager)} 类型字段");
                        }
                        SetInstanceField(fieldTypeEnum, fieldType, instance, f);
                        continue;
                    }
                }
            }
        }

        //View 类通常需要继承 MonoBehaviour, 对象创建不受框架控制, 因此要从 Unity 获取作为启动参数传入
        private void StartWithViews(Assembly assembly, IView[] views)
        {
            if (!_hasStarted)
            {
                Debug.Log("-------------- Context 开始执行 --------------");
                DateTime startTime = DateTime.Now;
                if (views.Length != 0)
                {
                    foreach (IView v in views)
                    {
                        v.SetContext(Instance);;
                        IocContainer.Add(TypeEnum.View, v);
                    }
                }
                //扫描自定义的 assembly 并添加进 IOC 容器
                ScanTypeAttributesByAssembly(assembly);
                Assembly frameworkAssembly = Assembly.GetAssembly(typeof(T));
                //扫描 WillFrameworkPro 的所有类，筛选后添加进 IOC 容器
                ScanTypeAttributesByAssembly(frameworkAssembly);
                //注入依赖 + 调用初始化
                HandleIdentities();
                Debug.Log($"-------------- Context 执行完毕, 用时: {(DateTime.Now - startTime).Milliseconds} ms --------------");
                Debug.Log(_iocContainer);
                Debug.Log(CommandContainer);
                //启动过后更新状态为：已启动
                _hasStarted = true;
            }
        }

        public void StartWithViewsOnSceneLoading(Assembly localAssembly, params IView[] views)
        {
            _hasStarted = false;
            StartWithViews(localAssembly, views);
        }
        /// <summary>
        /// 清空所有的容器
        /// </summary>
        public void ClearContainers()
        {
            _commandContainer.Clear();
            _iocContainer.Clear();
            _stateContainer.Clear();
        }
    }
}

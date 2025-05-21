using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Attributes.Injection;
using WillFrameworkPro.Core.Context;
using WillFrameworkPro.Core.Views;
using WillFrameworkPro.Tools.TagManager;

namespace WillFrameworkPro.Core
{
    /// <summary>
    /// 注意：这个基类必须要被继承，不能直接使用。
    /// </summary>
    public abstract class BaseApplication : MonoBehaviour
    {
        protected IContext Context { get => WillFrameworkPro.Core.Context.Context.Instance; }
        
        protected virtual void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        protected virtual void Update()
        {
            PrintCommandContainerByKeyDown();
        }
        /// <summary>
        /// 找出场景中的所有对象，包含非激活的
        /// </summary>
        /// <param name="localAssembly">当前场景代码所属的 assembly</param>
        /// <returns></returns>
        private BaseView[] ScanViewsInTheScene(out Assembly localAssembly)
        {
            //查找出场景中的所有对象，包含非激活的，不排序
            localAssembly = GetType().Assembly;
            var sceneObjectList = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<BaseView> resultViewList = new();
            foreach (var so in sceneObjectList)
            {
                //若有 tag，通过 tagManager 将其缓存起来，留作后面使用
                TagManager.RegisterObject(so);
                //同一个 gameObject 可能会挂载多个脚本 view 对象
                BaseView[] views = so.GetComponents<BaseView>();
                foreach (var v in views)
                {
                    //保存到 view 列表
                    resultViewList.Add(v);
                }
            }
            
            return resultViewList.ToArray();
        }

        private void OnActiveSceneChanged(Scene arg1, Scene arg2)
        {
            Context.CommandContainer.Dispose();
            Context.IocContainer.Dispose();
            BaseView[] views = ScanViewsInTheScene(out Assembly localAssembly);
            Context.StartWithViewsOnSceneLoading(localAssembly, views);
        }

        protected virtual void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
        
        /// <summary>
        /// 打印 command 事件容器：
        ///     按下 ctrl + C + M 按键触发打印一次
        /// </summary>
        public virtual void PrintCommandContainerByKeyDown()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log(Context.CommandContainer);
            }
        }
    }
}
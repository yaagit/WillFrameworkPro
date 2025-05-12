using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Attributes.Types.Injection;
using WillFrameworkPro.Core.Context;
using WillFrameworkPro.Core.Views;
using WillFrameworkPro.Tools.TagManager;

namespace WillFrameworkPro.Core
{
    public class BaseApplication : MonoBehaviour
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
            // //可以找到 inactive 状态的对象
            // BaseView[] views = Resources.FindObjectsOfTypeAll<BaseView>();
            // localAssembly = GetType().Assembly;
            // List<BaseView> resultViewList = new();
            // //Resources.FindObjectsOfTypeAll 会找到上一个场景的预制件,因此要过滤掉
            // foreach (var view in views)
            // {
            //     Assembly viewAssembly = view.GetType().Assembly;
            //     //view 源文件只有两种源头：1.用户的自定义 Assembly 2.属于本框架的 Assembly
            //     Assembly frameworkAssembly = Assembly.GetAssembly(typeof(BaseApplication));
            //     if (viewAssembly == localAssembly || viewAssembly == frameworkAssembly)
            //     {
            //         resultViewList.Add(view);
            //     }
            // }
            //查找出场景中的所有对象，包含非激活的，不排序
            localAssembly = GetType().Assembly;
            var sceneObjectList = FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            List<BaseView> resultViewList = new();
            foreach (var so in sceneObjectList)
            {
                //若有 tag，通过 tagManager 将其缓存起来，留作后面使用
                TagManager.RegisterObject(so);
                //保存到 view 列表
                BaseView view = so.GetComponent<BaseView>();
                if (view != null)
                {
                    resultViewList.Add(view);
                }
            }
            
            return resultViewList.ToArray();
        }

        void OnActiveSceneChanged(Scene arg1, Scene arg2)
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
        public void PrintCommandContainerByKeyDown()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log(Context.CommandContainer);
            }
        }
    }
}
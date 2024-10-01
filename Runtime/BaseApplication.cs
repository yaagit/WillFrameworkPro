using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using WillFrameworkPro.Runtime.Context;
using WillFrameworkPro.Runtime.Tiers;

namespace WillFrameworkPro.Runtime
{
    public class BaseApplication : MonoBehaviour
    {
        protected IContext Context { get => WillFrameworkPro.Runtime.Context.Context.Instance; }

        protected virtual void Awake()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        
        private BaseView[] ScanViewsInTheScene(out Assembly localAssembly)
        {
            //可以找到 inactive 状态的对象
            BaseView[] views = Resources.FindObjectsOfTypeAll<BaseView>();
            localAssembly = GetType().Assembly;
            List<BaseView> resultViewList = new();
            //Resources.FindObjectsOfTypeAll 会找到上一个场景的预制件,因此要过滤掉
            foreach (var view in views)
            {
                Assembly viewAssembly = view.GetType().Assembly;
                //view 源文件只有两种源头：1.用户的自定义 Assembly 2.属于本框架的 Assembly
                Assembly frameworkAssembly = Assembly.GetAssembly(typeof(BaseApplication));
                if (viewAssembly == localAssembly || viewAssembly == frameworkAssembly)
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
    }
}
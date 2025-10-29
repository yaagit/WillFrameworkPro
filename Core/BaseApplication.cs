using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            PrintContainerByKeyDown();
        }
        /// <summary>
        /// 找出场景中的所有对象，包含非激活的
        /// </summary>
        private BaseView[] ScanViewsInTheScene()
        {
            //查找出场景中的所有对象，包含非激活的，不排序
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
            //每次场景加载，都先清空容器。
            Context.ClearContainers();
            
            BaseView[] views = ScanViewsInTheScene();
            Context.StartWithViewsOnSceneLoading(views);
        }

        protected virtual void OnDestroy()
        {
            //每次场景切换，都会销毁所有的 gameObject，本类也不例外。因此，要在销毁方法中注销绑定的事件，防止注册多个重复的 activeSceneChanged 事件和对象得不到回收导致的内存泄露。
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
        
        /// <summary>
        /// 打印 command 事件容器：
        ///     按下 ctrl + C + M 按键触发打印一次
        /// </summary>
        public virtual void PrintContainerByKeyDown()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.C) && Input.GetKeyDown(KeyCode.M))
            {
                Debug.Log(Context.IocContainer);
                Debug.Log(Context.CommandContainer);
                Debug.Log(Context.StateContainer);
            }
        }
    }
}
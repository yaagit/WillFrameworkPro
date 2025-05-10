using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Context;
using WillFrameworkPro.Core.Rules;
using Object = UnityEngine.Object;

namespace WillFrameworkPro.Core.Tiers
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        private IContext _context;
        
        protected virtual void OnDestroy()
        {
            _context.IocContainer.Remove(IdentityType.View, this);
            _context.CommandContainer.UnbindEvents(this);
        }
        /// <summary>
        /// 需要子类重写此方法，在 Context 加载时自动调用，执行时机等同于 Awake。
        /// </summary>
        public virtual void Initialize() { }

        IContext ICanGetContext.GetContext()
        {
            return _context;
        }

        void ICanSetContext.SetContext(IContext context)
        {
            _context = context;
        }
        protected new T Instantiate<T>(T original) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original);
            HandleInstantiated(instance);
            return instance;
        }
        protected new T Instantiate<T>(T original, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent);
            HandleInstantiated(instance);
            return instance;
        }
        protected new T Instantiate<T>(T original, Transform parent, bool instantiateInWorldSpace) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, parent, instantiateInWorldSpace);
            HandleInstantiated(instance);
            return instance;
        }
        protected new T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation);
            HandleInstantiated(instance);
            return instance;
        }
        protected new T Instantiate<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object
        {
            T instance = MonoBehaviour.Instantiate(original, position, rotation, parent);
            HandleInstantiated(instance);
            return instance;
        }

        private void HandleInstantiated<T>(T instance) where T : Object
        {
            IView view = instance as IView;
            if (view != null)
            {
                _context.PresetGeneratedView(view);
            }
            else
            {
                GameObject go = instance as GameObject;
                view = go.GetComponent<IView>();
                if (view != null)
                {
                    _context.PresetGeneratedView(view);
                }
            }
        }
    }
}
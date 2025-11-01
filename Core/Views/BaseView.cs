// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System.Linq;
using UnityEngine;
using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Context;
using WillFrameworkPro.Core.Rules;
using Object = UnityEngine.Object;

namespace WillFrameworkPro.Core.Views
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        protected IContext _context;
        
        protected virtual void OnDestroy()
        {
            _context.IocContainer.Remove(TypeEnum.View, this);
            _context.CommandContainer.UnbindEvents(this);
        }

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
            if (instance == null)
                return;
            // 只处理 GameObject 实例（Material 可以被实例化，但不属于 GameObject）
            if (instance is GameObject go)
            {
                // 包括根对象和所有子对象上的所有 IView
                var allViews = go.GetComponents<IView>()
                    .Concat(go.GetComponentsInChildren<IView>(true))
                    .Distinct();
                foreach (var view in allViews)
                {
                    _context.PresetGeneratedView(view);
                }
            }
        }
    }
}
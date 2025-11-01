// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using WillFrameworkPro.Extensions.Tools.Observable.Events;

namespace WillFrameworkPro.Extensions.Tools.Observable
{
    public abstract class BaseObservable<T>
    {
        protected readonly IEvent<T, T> OnValueChanged;
        
        protected T _currentValue;
        protected T _previousValue;
        
        public BaseObservable()
        {
            OnValueChanged = new EventImpl<T, T>();
        }
        
        public abstract T Value { get; set; }
        
        
        public void AddListener(EventHandler<T, T> call, T oldValue = default(T), T newValue = default(T))
        {
            OnValueChanged.AddListener(call, false, oldValue, newValue);
        }

        public void RemoveListener(EventHandler<T, T> call)
        {
            OnValueChanged.RemoveListener(call);
        }
    }
}
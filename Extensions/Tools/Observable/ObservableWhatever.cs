// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
namespace WillFrameworkPro.Extensions.Tools.Observable
{
    public class ObservableWhatever<T> : BaseObservable<T>
    {
        public override T Value
        {
            set
            {
                _previousValue = _currentValue;
                _currentValue = value;
                //赋值后立即调用
                OnValueChanged.Invoke(_previousValue, _currentValue);
            }
            get
            {
                return _currentValue;
            }
        }
        
    }
}

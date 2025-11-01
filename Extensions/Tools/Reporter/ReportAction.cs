// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System;

namespace WillFrameworkPro.Extensions.Tools.Reporter
{
    public class ReportAction<T>
    {
        private Action<T> _action;

        public void AddListener(Action<T> action)
        {
            _action += action;
        }

        public void RemoveListener(Action<T> action)
        {
            _action -= action;
        }

        public void Trigger(T p)
        {
            _action?.Invoke(p);
        }
    }
    
    public class ReportAction
    {
        private Action _action;

        public void AddListener(Action action)
        {
            _action += action;
        }

        public void RemoveListener(Action action)
        {
            _action -= action;
        }

        public void Trigger()
        {
            _action?.Invoke();
        }
    }
}
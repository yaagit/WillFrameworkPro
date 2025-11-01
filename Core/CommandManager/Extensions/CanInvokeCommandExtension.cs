// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager.Extensions
{
    public static class CanInvokeCommandExtension
    {
        public static void InvokeCommand(this ICanInvokeCommand self, ICommand command)
        {
            if (self == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.InvokeCommand(command);
        }
        public static void ErrorWarning()
        {
            Debug.LogError("检测到 Context 的引用为空,可能是你在 Monobehavior 的 Awake 方法内引用了 CommandManager 对象, 解决方式: 请在 IAutoInitialize 接口的 AutoInitialize 方法内引用此对象或在 Monobehavior 的 Start 方法内引用此对象.");
        }
    }
}
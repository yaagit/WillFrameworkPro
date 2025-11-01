// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;
using WillFrameworkPro.Core.CommandManager;
using WillFrameworkPro.Core.Containers;

namespace WillFrameworkPro.Core.Views.Extensions
{
    public static class ViewExtension
    {
        public static void InvokeCommand(this IView self, ICommand command)
        {
            if (self == null || self.GetContext() == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.InvokeCommand(command);
        }
        
        public static void AddCommandListener<T>(this IView self, CommandContainer.InvokeCommandDelegate<T> del) where T : ICommand
        {
            if (self == null || self.GetContext() == null)
            {
                ErrorWarning();
            }
            self.GetContext().CommandContainer.AddCommandListener(self, del);
        }
        
        public static void ErrorWarning()
        {
            Debug.LogError("检测到 Context 的引用为空,可能是你在 Monobehavior 的 Awake 方法内调用了 CommandContainer 的方法.");
        }
    }
}
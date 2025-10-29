using System;
using System.Reflection;
using WillFrameworkPro.Core.Containers;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Core.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        StateContainer StateContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(params IView[] views);

        void ClearContainers();
    }
}
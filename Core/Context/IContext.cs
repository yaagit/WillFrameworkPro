using System.Reflection;
using WillFrameworkPro.Core.Containers;
using WillFrameworkPro.Core.Tiers;

namespace WillFrameworkPro.Core.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(Assembly localAssembly, params IView[] views);
    }
}
using System.Reflection;
using WillFrameworkPro.Containers;
using WillFrameworkPro.Tiers;

namespace WillFrameworkPro.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(Assembly localAssembly, params IView[] views);
    }
}
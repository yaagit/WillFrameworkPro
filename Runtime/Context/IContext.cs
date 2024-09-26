using System.Reflection;
using WillFrameworkPro.Runtime.Containers;
using WillFrameworkPro.Runtime.Tiers;

namespace WillFrameworkPro.Runtime.Context
{
    public interface IContext
    {
        IocContainer IocContainer { get; }
        
        CommandContainer CommandContainer { get; }
        
        void PresetGeneratedView(IView view);

        void StartWithViewsOnSceneLoading(Assembly localAssembly, params IView[] views);
    }
}
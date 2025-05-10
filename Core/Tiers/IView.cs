using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.Tiers
{
    public interface IView : ICanGetContext, ICanSetContext
    {
        void Initialize();
    }
}
using WillFrameworkPro.Runtime.Rules;

namespace WillFrameworkPro.Runtime.Tiers
{
    public interface IView : ICanGetContext, ICanSetContext
    {
        void Initialize();
    }
}
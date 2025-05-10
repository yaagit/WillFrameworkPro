using WillFrameworkPro.Core.Context;

namespace WillFrameworkPro.Core.Rules
{
    public interface ICanSetContext
    {
        void SetContext(IContext context);
    }
}
using WillFrameworkPro.Core.Attributes;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [Identity]
    public class HighLevelCommandManager : BaseCommandManager, ICanInvokeCommand
    {
        private HighLevelCommandManager() {}
    }
}
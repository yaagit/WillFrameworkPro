using WillFrameworkPro.Attributes;
using WillFrameworkPro.Rules;

namespace WillFrameworkPro.CommandManager
{
    [Identity]
    public class HighLevelCommandManager : BaseCommandManager, ICanInvokeCommand
    {
        private HighLevelCommandManager() {}
    }
}
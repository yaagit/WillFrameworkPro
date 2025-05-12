using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [General]
    public class HighLevelCommandManager : BaseCommandManager, ICanInvokeCommand
    {
        private HighLevelCommandManager() {}
    }
}
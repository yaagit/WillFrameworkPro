using WillFrameworkPro.Core.Attributes;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [Identity]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}
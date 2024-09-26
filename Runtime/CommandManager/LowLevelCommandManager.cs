using WillFrameworkPro.Runtime.Attributes;
using WillFrameworkPro.Runtime.Rules;

namespace WillFrameworkPro.Runtime.CommandManager
{
    [Identity]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}
using WillFrameworkPro.Attributes;
using WillFrameworkPro.Rules;

namespace WillFrameworkPro.CommandManager
{
    [Identity]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}
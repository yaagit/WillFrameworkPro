using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [General]
    public class LowLevelCommandManager : BaseCommandManager, ICanListenCommand
    {
        private LowLevelCommandManager() {}
    }
}
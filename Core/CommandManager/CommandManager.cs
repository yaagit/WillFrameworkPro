using WillFrameworkPro.Core.Attributes.Types;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [General]
    public class CommandManager : BaseCommandManager, ICanInvokeCommand, ICanListenCommand
    {
        private CommandManager() {}
    }
}
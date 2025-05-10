using WillFrameworkPro.Core.Attributes;
using WillFrameworkPro.Core.Rules;

namespace WillFrameworkPro.Core.CommandManager
{
    [Identity]
    public class CommandManager : BaseCommandManager, ICanInvokeCommand, ICanListenCommand
    {
        private CommandManager() {}
    }
}
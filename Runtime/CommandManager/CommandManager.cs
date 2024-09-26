using WillFrameworkPro.Runtime.Attributes;
using WillFrameworkPro.Runtime.Rules;

namespace WillFrameworkPro.Runtime.CommandManager
{
    [Identity]
    public class CommandManager : BaseCommandManager, ICanInvokeCommand, ICanListenCommand
    {
        private CommandManager() {}
    }
}
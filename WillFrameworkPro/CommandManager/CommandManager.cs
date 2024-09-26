using WillFrameworkPro.Attributes;
using WillFrameworkPro.Rules;

namespace WillFrameworkPro.CommandManager
{
    [Identity]
    public class CommandManager : BaseCommandManager, ICanInvokeCommand, ICanListenCommand
    {
        private CommandManager() {}
    }
}
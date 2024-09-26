using WillFrameworkPro.Context;
using WillFrameworkPro.Rules;

namespace WillFrameworkPro.CommandManager
{
    public class BaseCommandManager : ICommandManager, ICanSetContext, ICanGetContext
    {

        private IContext _context;
        
        void ICanSetContext.SetContext(IContext context)
        {
            _context = context;
        }

        IContext ICanGetContext.GetContext()
        {
            return _context;
        }
        
    }
}
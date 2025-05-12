using System;

namespace WillFrameworkPro.Core.Attributes.Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : BaseAttribute
    {
        public ControllerAttribute() : base(TypeEnum.Controller)
        {
            
        }
    }
}
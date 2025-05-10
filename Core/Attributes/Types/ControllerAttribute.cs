using System;
using WillFrameworkPro.Core.Attributes.Types;

namespace WillFrameworkPro.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : IdentityAttribute
    {
        public ControllerAttribute() : base(IdentityType.Controller)
        {
            
        }
    }
}
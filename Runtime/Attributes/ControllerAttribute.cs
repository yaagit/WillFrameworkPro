using System;
using WillFrameworkPro.Runtime.Attributes.Types;

namespace WillFrameworkPro.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : IdentityAttribute
    {
        public ControllerAttribute() : base(IdentityType.Controller)
        {
            
        }
    }
}
using System;
using WillFrameworkPro.Attributes.Types;

namespace WillFrameworkPro.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAttribute : IdentityAttribute
    {
        public ControllerAttribute() : base(IdentityType.Controller)
        {
            
        }
    }
}
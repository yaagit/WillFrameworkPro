using System;
using WillFrameworkPro.Runtime.Attributes.Types;

namespace WillFrameworkPro.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : IdentityAttribute
    {
        public ModelAttribute() : base(IdentityType.Model)
        {
            
        }
    }
}
using System;
using WillFrameworkPro.Core.Attributes.Types;

namespace WillFrameworkPro.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModelAttribute : IdentityAttribute
    {
        public ModelAttribute() : base(IdentityType.Model)
        {
            
        }
    }
}
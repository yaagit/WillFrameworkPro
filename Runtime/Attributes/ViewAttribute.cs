using System;
using WillFrameworkPro.Runtime.Attributes.Types;
namespace WillFrameworkPro.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : IdentityAttribute
    {
        public ViewAttribute() : base(IdentityType.View)
        {
        }
    }
}
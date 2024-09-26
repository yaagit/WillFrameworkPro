using System;
using WillFrameworkPro.Attributes.Types;
namespace WillFrameworkPro.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : IdentityAttribute
    {
        public ViewAttribute() : base(IdentityType.View)
        {
        }
    }
}
using System;
using WillFrameworkPro.Core.Attributes.Types;
namespace WillFrameworkPro.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : IdentityAttribute
    {
        public ViewAttribute() : base(IdentityType.View)
        {
        }
    }
}
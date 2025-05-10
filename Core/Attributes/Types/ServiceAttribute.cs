using System;
using WillFrameworkPro.Core.Attributes.Types;
namespace WillFrameworkPro.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : IdentityAttribute
    {
        public ServiceAttribute() : base(IdentityType.Service)
        {
        }
    }
}
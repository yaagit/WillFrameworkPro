using System;
using WillFrameworkPro.Runtime.Attributes.Types;
namespace WillFrameworkPro.Runtime.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : IdentityAttribute
    {
        public ServiceAttribute() : base(IdentityType.Service)
        {
        }
    }
}
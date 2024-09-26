using System;
using WillFrameworkPro.Attributes.Types;
namespace WillFrameworkPro.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : IdentityAttribute
    {
        public ServiceAttribute() : base(IdentityType.Service)
        {
        }
    }
}
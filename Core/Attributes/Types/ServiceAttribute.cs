using System;
namespace WillFrameworkPro.Core.Attributes.Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServiceAttribute : BaseAttribute
    {
        public ServiceAttribute() : base(TypeEnum.Service)
        {
        }
    }
}
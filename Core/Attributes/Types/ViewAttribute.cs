using System;
namespace WillFrameworkPro.Core.Attributes.Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ViewAttribute : BaseAttribute
    {
        public ViewAttribute() : base(TypeEnum.View)
        {
        }
    }
}
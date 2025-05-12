using System;
namespace WillFrameworkPro.Core.Attributes.Types
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class GeneralAttribute : BaseAttribute
    {
        public GeneralAttribute() : base(TypeEnum.General)
        {
        }
    }
}
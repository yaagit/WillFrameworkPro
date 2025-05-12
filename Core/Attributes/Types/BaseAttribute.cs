using System;
namespace WillFrameworkPro.Core.Attributes.Types
{
    /// <summary>
    /// 类型的基类 Attribute，只作子类的基类，不能他用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BaseAttribute : Attribute
    {
        private TypeEnum _typeEnum;
        public TypeEnum TypeEnum { get => _typeEnum; }

        public BaseAttribute(TypeEnum typeEnum)
        {
            _typeEnum = typeEnum;
        }
    }
}
using UnityEngine;

namespace WillFrameworkPro.Attributes.ReadOnly
{
    /// <summary>
    /// 在继承了 MonoBehavior 的类字段上标注 [ReadOnly] 特性，则表示这个字段是不可修改的，仅作展示。
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
        
    }
}
// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
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
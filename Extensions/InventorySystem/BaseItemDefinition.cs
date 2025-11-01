// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEditor;
using UnityEngine;
using WillFrameworkPro.Attributes.ReadOnly;

namespace WillFrameworkPro.Extensions.InventorySystem
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class BaseItemDefinition : ScriptableObject
    {
        public ItemCategory Category;//item 所属种类
        [ReadOnly]
        public string ID;
        public string Name;
        public Sprite Icon;
        public bool IsStackable;//是否可堆叠？false 时，每一个物品都单独占用一个格子，不会堆叠在同一个格子然后数量显示。
        public int MaxStack = 99;//最高可堆叠数量
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            // 仅在编辑器中首次创建时生成 ID，不在运行时生成
            if (string.IsNullOrEmpty(ID))
            {
                ID = GUID.Generate().ToString(); // UnityEditor.GUID 比 System.Guid 更符合编辑器习惯
                //标记当前对象 this 为“已修改”，告诉 Unity 需要保存它的状态
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        #endif
    }
}

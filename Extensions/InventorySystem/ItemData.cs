// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace WillFrameworkPro.Extensions.InventorySystem
{
    [System.Serializable]
    public class ItemData
    {
        public int Amount;//掉落物的数量
        
        public string ID;
        public string Name;
        public Sprite Icon;
        public bool IsStackable; //是否可堆叠？false 时，每一个物品都单独占用一个格子，不会堆叠在同一个格子然后数量显示。
        public int MaxStack; //最高可堆叠数量
        public ItemCategory Category;//item 所属种类
        public ItemData(BaseItemDefinition def, int amount)
        {
            ID = def.ID;
            Name = def.Name;
            Icon = def.Icon;
            IsStackable = def.IsStackable;
            MaxStack = def.MaxStack;
            Amount = amount;
            Category = def.Category;
        }
    }
}
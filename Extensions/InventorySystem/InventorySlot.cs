// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0

namespace WillFrameworkPro.Extensions.InventorySystem
{
    /// <summary>
    /// 库存物品栏中的插槽
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        public ItemData ItemData { get; private set; }
        public int Quantity { get; private set; } // 该插槽堆叠的物品数量

        /// <summary>
        /// 插槽是否为空？
        /// </summary>
        public bool IsEmpty => ItemData == null || Quantity <= 0;
        
        /// <summary>
        /// 往插槽添加物品
        /// </summary>
        public void AddItem(ItemData newItem, int amount)
        {
            //插槽上没有物品
            if (ItemData == null)
            {
                ItemData = newItem;
                Quantity = amount;
                //item 不可堆叠，无论添加多少，数量始终为 1.
                if (!ItemData.IsStackable)
                {
                    Quantity = 1;
                }
                //若 item 可堆叠，则限制数量.
                else if (ItemData.IsStackable && Quantity > ItemData.MaxStack)
                {
                    Quantity = ItemData.MaxStack;
                }
            }
            //插槽上有物品
            else if (ItemData.ID == newItem.ID && ItemData.IsStackable)
            {
                Quantity += amount;
                if (Quantity > ItemData.MaxStack)
                {
                    Quantity = ItemData.MaxStack;
                }
            }
        }
        /// <summary>
        /// 清除当前插槽
        /// </summary>
        public void ClearSlot()
        {
            ItemData = null;
            Quantity = 0;
        }
    }
}
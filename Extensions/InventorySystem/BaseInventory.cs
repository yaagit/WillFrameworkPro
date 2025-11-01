// Copyright 2025 Will Chan
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
using System;
using System.Collections.Generic;
using UnityEngine;
using WillFrameworkPro.Core.Views;

namespace WillFrameworkPro.Extensions.InventorySystem
{
    public class BaseInventory : BaseView
    {
        private List<InventorySlot> _slots = new();
        public List<InventorySlot> Slots { get => _slots;}
        
        [Header("初始化的物品栏插槽数量")]
        [SerializeField] 
        protected int _initializedSize;
        [Header("是否支持动态添加插槽？（true 表示超过初始化的物品栏槽位数量将会开辟出新的槽位）")]
        [SerializeField] 
        protected bool _canDynaAddSlot;
        [Header("同类型物品是否可以占用多个插槽？（true 表示槽位达到数量上限之后将会开辟出新的槽位）")]
        [SerializeField] 
        protected bool _isMultipleSlots;

        private void Awake()
        {
            //初始化插槽
            for (int i = 0; i < _initializedSize; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }
        /// <summary>
        /// 添加物品：
        ///     根据条件设置，是否启用动态添加插槽。
        /// </summary>
        public void AddItem(BaseItem item)
        {
            //MonoBehavior 不适宜作为数据存储的对象。从里面提取出可供数据存储的类型。
            ItemData itemData = item.ToItemData();
            int amount = itemData.Amount;
            
            if (_canDynaAddSlot)
            {
                DynaAddItem(itemData, amount);
            }
            else
            {
                RawAddItem(itemData, amount);
            }
        }
        
        /// <summary>
        /// 添加物品，同时动态根据物品的数量去添加插槽。
        /// </summary>
        private void DynaAddItem(ItemData itemData, int amount = 1)
        {
            int remaining = amount;
            while (remaining > 0)
            {
                int left = RawAddItem(itemData, remaining);
                if (left == 0)
                {
                    break;
                }
                AddSlots(); // 添加一个新插槽
                if (_slots.Count > 99) // 避免无限添加
                {
                    Debug.LogWarning("Too many slots, aborting to prevent overflow.");
                    break;
                }
                remaining = left;
            }
        }
        
        /// <summary>
        /// 添加插槽
        /// </summary>
        private void AddSlots(int amount = 1)
        {
            if (amount <= 0)
            {
                return;
            }
            for (int i = 0; i < amount; i++)
            {
                _slots.Add(new InventorySlot());
            }
        }
        
        /// <summary>
        /// 添加物品，返回尝试添加后剩余的物品数量
        /// </summary>
        private int RawAddItem(ItemData itemData, int amount = 1)
        {
            int remaining = amount;
            
            if (itemData.IsStackable)
            {
                // 先找相同可叠加的物品
                foreach (var slot in _slots)
                {
                    if (!slot.IsEmpty && slot.ItemData.ID == itemData.ID)
                    {
                        //比较当前插槽的可用数量与剩余数量的值
                        int canAdd = Math.Min(itemData.MaxStack - slot.Quantity, remaining);
                        if (canAdd > 0)
                        {
                            slot.AddItem(itemData, canAdd);
                            remaining -= canAdd;
                            if (remaining <= 0)
                            {
                                return 0;
                            }
                            // 若 remaining > 0。同时，规定了只能占用一个插槽。
                            if (!_isMultipleSlots)
                            {
                                // 如果不允许多个插槽，仅在一个槽中放入后就停止
                                return 0;
                            }
                        }
                        //当前已经占用了一个插槽，且已经不可再添加数量。
                        else
                        {
                            // 若规定了只能占用一个插槽，则直接返回
                            if (!_isMultipleSlots)
                            {
                                return 0;
                            }
                        }
                    }
                }
            }
            
            //再找空插槽
            foreach (var slot in _slots)
            {
                if (slot.IsEmpty)
                {
                    int canAdd = Math.Min(itemData.MaxStack, remaining);
                    slot.AddItem(itemData, canAdd);
                    remaining -= canAdd;
                    if (remaining <= 0)
                    {
                        return 0;
                    }
                }
            }
            return remaining; // 插槽已满
        }
        
    }
}
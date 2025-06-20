using System;
using UnityEngine;
using WillFrameworkPro.Core.Views;
using WillFrameworkPro.Editor.ReadOnly;

namespace WillFrameworkPro.InventorySystem
{
    public enum ItemCategory
    {
        None, FreeHand, MeleeWeapon, GunWeapon, Ammo, Health, KeyItem
    }
    
    /// <summary>
    /// 库存物品的基类
    /// </summary>
    [Serializable]
    public class BaseItem : BaseView
    {
        [SerializeField]
        private BaseItemDefinition _itemDefinition; //Item 的定义，这是一个 ScriptObject 对象。定义了 Item 的基本属性。

        [SerializeField] 
        private int _amount; //当前掉落物的数量
        /// <summary>
        /// 以下表示 Item 基本属性的字段皆由 ItemDefinition 自动填充。仅作展示，不能在 inspector 修改。
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private string _ID;
        [ReadOnly]
        [SerializeField]
        private string _name;
        [ReadOnly]
        [SerializeField]
        private Sprite _icon;
        [ReadOnly]
        [SerializeField]
        private bool _isStackable;//是否可堆叠？false 时，每一个物品都单独占用一个格子，不会堆叠在同一个格子然后数量显示。
        [ReadOnly]
        [SerializeField]
        private int _maxStack;//最高可堆叠数量
        [ReadOnly]
        [SerializeField]
        private ItemCategory _category;//item 所属种类
        
        private void Awake()
        {
            _ID = _itemDefinition.ID;
            _name = _itemDefinition.Name;
            _icon = _itemDefinition.Icon;
            _isStackable = _itemDefinition.IsStackable;
            _maxStack = _itemDefinition.MaxStack;
            _category = _itemDefinition.Category;
        }
        /// <summary>
        /// 将数据转换为可以提供给 Slot 存储的数据形式：ItemData.
        /// </summary>
        public ItemData ToItemData()
        {
            return new ItemData(_itemDefinition, _amount);
        }
    }
}
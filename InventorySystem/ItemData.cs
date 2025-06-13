using UnityEngine;

namespace WillFrameworkPro.InventorySystem
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
        public ItemData(BaseItemDefinition def, int amount)
        {
            ID = def.ID;
            Name = def.Name;
            Icon = def.Icon;
            IsStackable = def.IsStackable;
            MaxStack = def.MaxStack;
            Amount = amount;
        }
    }
}
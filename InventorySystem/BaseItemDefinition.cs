using System;
using UnityEngine;

namespace WillFrameworkPro.InventorySystem
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
    public class BaseItemDefinition : ScriptableObject
    {
        public string ID;
        public string Name;
        public Sprite Icon;
        public bool IsStackable;//是否可堆叠？false 时，每一个物品都单独占用一个格子，不会堆叠在同一个格子然后数量显示。
        public int MaxStack = 99;//最高可堆叠数量
        
        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = Guid.NewGuid().ToString("N"); //去掉 "-"
                //标记当前对象 this 为“已修改”，告诉 Unity 需要保存它的状态
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }
        #endif
    }
}

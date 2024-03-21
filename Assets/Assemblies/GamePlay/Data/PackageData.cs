using System;
using System.Collections.Generic;
using MemoryPack;
using Newtonsoft.Json;

namespace Game
{
    // 玩家背包数据
    [MemoryPackable, Serializable]
    public partial class PackageData
    {
        [Sirenix.OdinInspector.ShowInInspector]
        [JsonProperty]
        private Dictionary<ItemType, List<ItemData>> _itemsByType;
        public PackageData()
        {
            _itemsByType = new Dictionary<ItemType, List<ItemData>>();
            foreach (var value in Enum.GetValues(typeof(ItemType)))
            {
                _itemsByType.Add((ItemType)value, new List<ItemData>());
            }
        }
        
        public IEnumerable<ItemData> GetEnumerator(ItemType type)
        {
            return _itemsByType[type];
        }


        public void AddItem(ItemEnum id, int count)
        {
            // TODO: Add item to package
            ItemConfig config = GlobalManager.Singleton.configTable.GetItemConfig(id);
            List<ItemData> itemData = _itemsByType[config.type];
            itemData.Add(new ItemData()
            {
                id = id,
                count = count
            });
        }

        public void RemoveItem(ItemEnum id, int count)
        {
            // GlobalManager.Singleton.configTable.GetItemConfig(id);
            // TODO: Remove item from package
        }


        public ItemData Get(ItemType currentType, int idx)
        {
            return _itemsByType[currentType][idx];
        }

        public int ItemCount(ItemType type)
        {
            return _itemsByType[type].Count;
        }
    }
}
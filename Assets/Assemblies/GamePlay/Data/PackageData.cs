using System;
using System.Collections.Generic;
using MemoryPack;

namespace Game
{
    // 玩家背包数据
    [MemoryPackable,Serializable]
    public partial class PackageData
    {
        public List<ItemData> items = new List<ItemData>();

        public void AddItem(int id, int count)
        {
            
        }

        public void RemoveItem(int id, int count)
        {
            
        }
    }
}
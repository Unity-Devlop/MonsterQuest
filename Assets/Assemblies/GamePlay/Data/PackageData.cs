using System.Collections.Generic;
using MemoryPack;

namespace Game
{
    // 玩家背包数据
    [MemoryPackable]
    public partial class PackageData
    {
        public List<ItemData> items = new List<ItemData>();
    }
}
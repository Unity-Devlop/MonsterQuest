using MemoryPack;

namespace Game
{
    [MemoryPackable]
    public partial struct ItemConfig
    {
        public int id;
        public string name => ((ItemEnum)id).ToString();
        public ItemType type;
    }
}
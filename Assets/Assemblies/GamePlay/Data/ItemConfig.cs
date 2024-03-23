using System;
using MemoryPack;

namespace Game
{
    [MemoryPackable,Serializable]
    public partial struct ItemConfig
    {
        public string name;
        public ItemType type;
#if UNITY_EDITOR
        [UnityEngine.TextArea]
#endif
        public string desc;

        public bool canStack;
        public int maxStack;
    }
}

using MemoryPack;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [MemoryPackable]
    public partial struct ItemConfig
    {
        public int id;
        public string name;
        public ItemType type;
    }


    [MemoryPackable]
    public partial struct ItemData
    {
        public int id;
        [MemoryPackIgnore] private ItemConfig _config;
        [MemoryPackIgnore] private bool _configInitialized;

        [MemoryPackIgnore]
        public ItemConfig config
        {
            get
            {
                if (!_configInitialized)
                {
                    _config = ConfigTable.Instance.GetItemConfig(id);
                    _configInitialized = true;
                }

                return _config;
            }
        }

        public int count;
    }
}
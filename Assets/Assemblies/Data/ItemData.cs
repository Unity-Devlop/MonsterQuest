using MemoryPack;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [MemoryPackable]
    public partial struct ItemData
    {
        public ItemEnum id;
        [MemoryPackIgnore] private ItemConfig _config;
        [MemoryPackIgnore] private bool _configInitialized;

        [MemoryPackIgnore]
        public ItemConfig config
        {
            get
            {
                if (!_configInitialized)
                {
                    _config =  GlobalManager.Singleton.configTable.GetItemConfig(id);
                    _configInitialized = true;
                }

                return _config;
            }
        }

        public int count;
    }
}
using System;
using MemoryPack;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [MemoryPackable, Serializable]
    public partial struct  ItemData
    {
        public ItemEnum id;
        [MemoryPackIgnore] public string name => config.name;
        [MemoryPackIgnore] public ItemType type => config.type;
        [MemoryPackIgnore] private ItemConfig _config;
        [MemoryPackIgnore] private bool _configInitialized;

        [MemoryPackIgnore]
        public ItemConfig config
        {
            get
            {
                if (!_configInitialized)
                {
                    _config = GlobalManager.Singleton.configTable.GetItemConfig(id);
                    _configInitialized = true;
                }

                return _config;
            }
        }

        public int count;
    }
}
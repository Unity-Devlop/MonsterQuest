using System;
using MemoryPack;
using Newtonsoft.Json;
using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [MemoryPackable, Serializable]
    public partial struct ItemData
    {
        public ItemEnum id;
        [MemoryPackIgnore, JsonIgnore] public string name => config.name;
        [MemoryPackIgnore, JsonIgnore] public ItemType type => config.type;
        [MemoryPackIgnore, JsonIgnore] private ItemConfig _config;
        [MemoryPackIgnore, JsonIgnore] private bool _configInitialized;

        [MemoryPackIgnore, JsonIgnore]
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
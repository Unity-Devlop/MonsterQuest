using Sirenix.Utilities;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [GlobalConfig("Assets/AddressablesResources/DataTable/"),
     CreateAssetMenu(fileName = "ConfigTable", menuName = "Game/ConfigTable")]
    public class ConfigTable : GlobalConfig<ConfigTable>
    {
        public SerializableDictionary<int, ItemConfig> itemConfigs;

        public ItemConfig GetItemConfig(int id)
        {
            return itemConfigs[id];
        }
    }
}
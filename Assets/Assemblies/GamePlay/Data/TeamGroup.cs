using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine;

namespace Game
{
    [MemoryPackable, Serializable]
    public partial struct TeamGroup
    {
        [MemoryPackIgnore, JsonIgnore] public static TeamGroup Default => new TeamGroup(0, "Default", Color.clear);
        public int id;
        public string name;
        public Color color;

        public TeamGroup(int id, string name, Color color)
        {
            this.id = id;
            this.name = name;
            this.color = color;
        }
    }
}
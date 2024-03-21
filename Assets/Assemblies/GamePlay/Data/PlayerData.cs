using System;
using MemoryPack;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Game
{
    [MemoryPackable, Serializable]
    public partial class PlayerData
    {
        public string userId;
        public string playerName;

        // public int teamId;
        // [MemoryPackIgnore]
        public int groupId;

        // self 
        public PokemonData self;

        public PokemonData carry1;// TODO null as default
        
        public PlayerData(string userId, string playerName, PokemonData self)
        {
            groupId = TeamGroup.Default.id;
            this.userId = userId;
            this.playerName = playerName;
            this.self = self;
        }
    }
}
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
        
        // [MemoryPackIgnore]
        public int teamId;

        // self 
        public PokemonData self;

        public PokemonData carry1; // TODO null as default

        public PlayerData(string userId, string playerName, int teamId, PokemonData self)
        {
            this.teamId = teamId;
            this.userId = userId;
            this.playerName = playerName;
            this.self = self;
        }
    }
}
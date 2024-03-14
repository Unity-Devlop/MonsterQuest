using System;
using MemoryPack;
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
        public TeamGroup group;

        // self 
        public PokemonData self;

        public PokemonData carry1;

        public PlayerData(string userId, string playerName, PokemonData self)
        {
            group = TeamGroup.Default;
            this.userId = userId;
            this.playerName = playerName;
            this.self = self;
        }
    }
}
using System;
using MemoryPack;

namespace Game
{
    [MemoryPackable,Serializable]
    public partial class PlayerData
    {
        public string userId;
        public string playerName;
        public int level;
        
        // pokemonData
        public float moveSpeed;
        public float runSpeed;
        public int currentPokemonId;
        public float rotateSpeed;
        
    }
    
    [MemoryPackable,Serializable]
    public partial class PokemonData
    {
        public int configId;
        public int uniqueId;
        public float moveSpeed;
        public float runSpeed;
        public float rotateSpeed;
        public float flySpeed;
    }
}
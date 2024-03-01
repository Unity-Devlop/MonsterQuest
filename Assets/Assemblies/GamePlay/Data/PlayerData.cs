using MemoryPack;

namespace Game
{
    [MemoryPackable]
    public partial class PlayerData
    {
        public string userId;
        public string userName;
        public int level;
        public float moveSpeed;
        public float runSpeed;
        public int currentPokemonId;
    }
}
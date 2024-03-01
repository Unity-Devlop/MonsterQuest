using MemoryPack;

namespace Game
{
    [MemoryPackable]
    public partial struct PublicChatMessage
    {
        public int senderId;
        public string content;
    }


}
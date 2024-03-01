using Mirror;

namespace Game
{
    public struct RequestEnterGameMessage : NetworkMessage
    {
        public string userId;
        public string userName;
    }
}
using UnityEngine;

namespace Game
{
    public class AuthenticationViewer : MonoBehaviour
    {
        public string playerName;
        
        
        public string password;
        public string userId;

        private void Awake()
        {
            Authentication.userId = userId;
            Authentication.password = password;
            Authentication.playerName = playerName;
        }
    }
}
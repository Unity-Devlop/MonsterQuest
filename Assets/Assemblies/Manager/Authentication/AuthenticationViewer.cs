using UnityEngine;

namespace Game
{
    public class AuthenticationViewer : MonoBehaviour
    {
        public string userName;
        public string userId;

        private void Awake()
        {
            Authentication.userName = userName;
            Authentication.userId = userId;
        }
    }
}
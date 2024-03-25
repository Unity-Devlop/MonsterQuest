using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FriendItem : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Button deleteButton;


        // public void Bind(int idx,FriendPair pair)
        // {
        //     nameText.text = pair.playerName;
        //     deleteButton.onClick.AddListener(() => { Player.Local.HandleDeleteFriend(pair.uid); });
        // }
    }
}
using System;
using Proto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FriendItem : MonoBehaviour
    {
        private TextMeshProUGUI _nameText;
        private Button _deleteButton;
        private UserInfo _friendInfo;

        private void Awake()
        {
            _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            _deleteButton = transform.Find("DeleteButton").GetComponent<Button>();
            _deleteButton.onClick.AddListener(OnDeleteButtonClick);
        }

        private async void OnDeleteButtonClick()
        {
            ErrorMessage errorMessage = await GrpcClient.GameService.DeleteFriendAsync(new DeleteFriendRequest
            {
                SenderUid = Player.Local.userId,
                TargetUid = _friendInfo.Uid
            });
            if (errorMessage.Code != StatusCode.Ok)
            {
                Debug.LogError(errorMessage.Content);
            }
            GlobalManager.EventSystem.Send(new OnFriendListChange());
        }

        public void Bind(UserInfo friendInfo, int idx)
        {
            _nameText.text = friendInfo.Name;
            this._friendInfo = friendInfo;
        }
    }
}
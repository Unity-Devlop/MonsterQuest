using System;
using Proto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class FriendSearchResult : MonoBehaviour
    {
        private TextMeshProUGUI _nameText;
        private Button _addButton;
        private SearchFriendResponse _response;

        private void Awake()
        {
            _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            _addButton = transform.Find("AddButton").GetComponent<Button>();
            _addButton.onClick.AddListener(OnAddButtonClick);
        }

        private async void OnAddButtonClick()
        {
            ErrorMessage response = await GrpcClient.GameService.AddFriendAsync(new AddFriendRequest()
            {
                SenderUid = Player.Local.userId,
                TargetUid = _response.TargetUid
            });
            if (response.Code != StatusCode.Ok)
            {
                Debug.LogError(response.Content);
            }
            else
            {
                _addButton.gameObject.SetActive(false);
                GlobalManager.EventSystem.Send<OnFriendListChange>();
            }
        }

        public void Bind(SearchFriendResponse response)
        {
            this._response = response;
            if (response == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            _nameText.text = response.TargetName;
            if (response.IsFriend)
            {
                _addButton.gameObject.SetActive(false);
            }
            else
            {
                _addButton.gameObject.SetActive(true);
            }
        }
    }
}
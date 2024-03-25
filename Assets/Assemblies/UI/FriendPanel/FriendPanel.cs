using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class FriendPanel : UIPanel
    {
        private LoopVerticalScrollRect _friendList;
        private EasyGameObjectPool _friendItemPool;

        private TMP_InputField _searchInput;
        private Button _searchButton;

        private void Awake()
        {
            // TODO
            _friendList = transform.Find("FriendList").GetComponent<LoopVerticalScrollRect>();
            _friendItemPool = _friendList.GetComponent<EasyGameObjectPool>();

            _friendList.ItemProvider = idx => _friendItemPool.Get();
            _friendList.itemRenderer = ItemRenderer;
            _friendList.ItemReturn = (trans) => _friendItemPool.Release(trans.gameObject);

            _searchButton = transform.Find("SearchButton").GetComponent<Button>();
            _searchInput = transform.Find("SearchInput").GetComponent<TMP_InputField>();
            _searchButton.onClick.AddListener(OnSearch);
        }

        private void OnSearch()
        {
            string uid = _searchInput.text;
            if (string.IsNullOrEmpty(uid))
            {
                return;
            }
        }

        public override void OnOpened()
        {
            base.OnOpened();
            OnFriendListChanged();
        }

        private void OnFriendListChanged()
        {

        }

        public override void OnClosed()
        {
            base.OnClosed();

        }

        private void ItemRenderer(Transform tran, int idx)
        {
            // TODO
        }
    }
}
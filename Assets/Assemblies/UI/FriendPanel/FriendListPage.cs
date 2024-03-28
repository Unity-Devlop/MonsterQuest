using System;
using Google.Protobuf.Collections;
using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class FriendListPage : MonoBehaviour, IUISubPanel
    {
        private LoopVerticalScrollRect _friendList;
        private EasyGameObjectPool _friendItemPool;
        private RepeatedField<UserInfo> _friendInfos;
        private float _refreshInterval = 10f;
        private Timer _refreshTimer;
        private bool _refreshing;

        private void Awake()
        {
            _friendList = transform.Find("FriendList").GetComponent<LoopVerticalScrollRect>();
            _friendItemPool = _friendList.GetComponent<EasyGameObjectPool>();

            _friendList.ItemProvider = idx => _friendItemPool.Get();
            _friendList.itemRenderer = ItemRenderer;
            _friendList.ItemReturn = (trans) => _friendItemPool.Release(trans.gameObject);
        }

        private void ItemRenderer(Transform transform1, int idx)
        {
            FriendItem item = transform1.GetComponent<FriendItem>();
            item.Bind(_friendInfos[idx], idx);
        }

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        public void Open()
        {
            GlobalManager.EventSystem.Register<OnFriendListChange>(RefreshFriendList);
            RefreshFriendList(default);
            _refreshTimer = Timer.Register(_refreshInterval, () => RefreshFriendList(default), isLooped: true);
            gameObject.SetActive(true);
        }


        private async void RefreshFriendList(OnFriendListChange _)
        {
            if (_refreshing)
            {
                Debug.LogWarning("好友列表正在刷新中,拒绝重复请求");
                return;
            }

            _refreshing = true;
            DateTime deadline = DateTime.UtcNow.AddSeconds(_refreshInterval / 2);
            FriendShipList friendShipList = await GrpcClient.GameService.GetFriendListAsync(new FriendListRequest
            {
                Uid = Player.Local.userId
            }, deadline: deadline);
            if (friendShipList.Error.Code == StatusCode.Error)
            {
                Debug.LogError(friendShipList.Error.Content);
                _refreshing = false;
                return;
            }

            _friendList.totalCount = friendShipList.List.Count;
            _friendInfos = friendShipList.List;
            _friendList.RefillCells();
            _refreshing = false;
        }


        public void Close()
        {
            _refreshTimer.Cancel();
            _refreshTimer = null;
            gameObject.SetActive(false);
            GlobalManager.EventSystem.UnRegister<OnFriendListChange>(RefreshFriendList);
        }
    }
}
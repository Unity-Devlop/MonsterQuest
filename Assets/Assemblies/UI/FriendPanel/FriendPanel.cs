using System;
using UnityToolkit;

namespace Game.UI
{
    public class FriendPanel : UIPanel
    {
        private LoopVerticalScrollRect _friendList;
        private EasyGameObjectPool _friendItemPool;


        private void Awake()
        {
            // TODO
            _friendList = transform.Find("FriendList").GetComponent<LoopVerticalScrollRect>();
            _friendItemPool = transform.Find("FriendList/FriendItemPool").GetComponent<EasyGameObjectPool>();
        }
    }
}
using System;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class GameSelectPanel : MonoBehaviour, IUISubPanel
    {
        private EasyGameObjectPool _pool;
        private LoopVerticalScrollRect _gameList;
        private Timer _refreshTimer;
        private RepeatedField<ServerInfo> serverList;

        private void Awake()
        {
            _gameList = GetComponent<LoopVerticalScrollRect>();
            _pool = GetComponentInChildren<EasyGameObjectPool>();

            _gameList.ItemReturn = trans => { _pool.Release(trans.gameObject); };
            _gameList.itemRenderer = OnItemRenderer;
            _gameList.ItemProvider = _ => _pool.Get();
        }

        private void OnItemRenderer(Transform transform1, int idx)
        {
            GameItem item = transform1.GetComponent<GameItem>();
            item.Bind(serverList[idx]);
        }

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        public async void Open()
        {
            gameObject.SetActive(true);
            // request game list
            // TODO 没有请求完时 关闭这个界面 会出现问题
            ServerListResponse response = await GrpcClient.GlobalService.GetServerListAsync(new ServerListRequest());
            serverList = response.ServerList;
            _gameList.totalCount = serverList.Count;
            _gameList.RefillCells();
        }


        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
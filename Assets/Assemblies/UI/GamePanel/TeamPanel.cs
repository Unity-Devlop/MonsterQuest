using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class TeamPanel : MonoBehaviour, IUISubPanel
    {
        private TeamListResponse _response;
        private EasyGameObjectPool _pool;
        private LoopVerticalScrollRect _list;
        private TeamInfo _localPlayerTeam;

        public void Init()
        {
            _list = transform.Find("List").GetComponent<LoopVerticalScrollRect>();
            _pool = _list.GetComponent<EasyGameObjectPool>();
            _list.ItemReturn = (trans) => { _pool.Release(trans.gameObject); };
            _list.ItemProvider = (idx) => _pool.Get();
            _list.itemRenderer = ItemRenderer;
        }

        private void ItemRenderer(Transform transform1, int idx)
        {
            var team = _response.List[idx];
            if (team.Id == Player.Local.data.teamId)
            {
                _localPlayerTeam = team;
            }

            var teamItem = transform1.GetComponent<TeamItem>();
            teamItem.SetData(team);
        }

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        // private Timer _timer;
        public void Open()
        {
            Player.Local.OnTeamChanged += OnTeamChanged;
            gameObject.SetActive(true);
            OnTeamChanged();
        }

        private async void OnTeamChanged()
        {
            var response = await GrpcClient.GameService.GetTeamListAsync(new TeamListRequest());
            if (response.Error.Code == StatusCode.Error)
            {
                Debug.LogError(response.Error.Content);
                return;
            }

            _response = response;
            _list.totalCount = response.List.Count;
            _list.RefillCells();
        }

        public void Close()
        {
            if (Player.Local != null)
            {
                Player.Local.OnTeamChanged -= OnTeamChanged;
            }
           
            gameObject.SetActive(false);
        }
    }
}
using Proto;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class TeamItem : MonoBehaviour
    {
        [SerializeField] private Image colorImage;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button exitButton;
        private TeamInfo _info;

        private void Awake()
        {
            joinButton.onClick.AddListener(OnJoinClick);
            exitButton.onClick.AddListener(OnExitClick);
        }

        private void OnExitClick()
        {
            Player.Local.CmdChangeTeam(PokemonServer.DefaultTeamId);
            Debug.Log("Exit");
        }

        private async void OnJoinClick()
        {
            Player.Local.CmdChangeTeam(_info.Id);
            Debug.Log("Join");
        }

        public void SetData(TeamInfo team)
        {
            _info = team;
            nameText.text = team.Name;
            Color color = team.Color.HexToColor();
            colorImage.color = color;
            if (Player.Local.data.teamId == team.Id)
            {
                exitButton.gameObject.SetActive(true);
                joinButton.gameObject.SetActive(false);
            }
            else
            {
                exitButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(true);
            }
        }
    }
}
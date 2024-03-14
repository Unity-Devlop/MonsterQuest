using System;
using TMPro;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class HUDPanel : MonoBehaviour, IUISubPanel
    {
        private ProgressBar _healthBar;
        private TextMeshProUGUI levelText;

        private void Awake()
        {
            _healthBar = transform.Find("HealthBar").GetComponent<ProgressBar>();
            levelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        }

        public void Open()
        {
            // Player.LocalPlayer.OnSwitchPokemon += OnSwitchPokemon;
            GlobalManager.EventSystem.Register<OnLocalPlayerHealthChange>(OnLocalPlayerHealthChanged);
            GlobalManager.EventSystem.Register<OnLocalPlayerLevelChange>(OnLocalPlayerLevelChanged);

            PokemonData data = Player.LocalPlayer.controller.data;
            _healthBar.SetWithoutNotify(data.currentHealth, 0, data.maxHealth);
            levelText.text = $"Lv.{data.level}";
            
            
            gameObject.SetActive(true);
            
        }

        public void Close()
        {
            if (Player.LocalPlayer != null)
            {
                // Player.LocalPlayer.OnSwitchPokemon -= OnSwitchPokemon;
            }

            if (GlobalManager.EventSystem != null)
            {
                GlobalManager.EventSystem.UnRegister<OnLocalPlayerHealthChange>(OnLocalPlayerHealthChanged);
                GlobalManager.EventSystem.UnRegister<OnLocalPlayerLevelChange>(OnLocalPlayerLevelChanged);
            }

            gameObject.SetActive(false);
        }

        private void OnLocalPlayerLevelChanged(OnLocalPlayerLevelChange obj)
        {
            levelText.text = $"Lv.{obj.currentLevel}";
        }


        private void OnLocalPlayerHealthChanged(OnLocalPlayerHealthChange e)
        {
            _healthBar.SetWithoutNotify(e.currentHealth, 0, e.maxHealth);
        }
    }
}
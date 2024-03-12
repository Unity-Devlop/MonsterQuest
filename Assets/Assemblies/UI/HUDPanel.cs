using System;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public struct OnLocalPlayerPokemonHealthChange : IEvent
    {
        public int currentHealth;
        public int maxHealth;
    }

    public class HUDPanel : MonoBehaviour, IUISubPanel
    {
        private ProgressBar _healthBar;
        private ProgressBar _expBar;

        private void Awake()
        {
            _healthBar = transform.Find("HealthBar").GetComponent<ProgressBar>();
            _expBar = transform.Find("ExpBar").GetComponent<ProgressBar>();
        }

        public void Open()
        {
            PlayerController.LocalPlayer.OnSwitchPokemon += OnSwitchPokemon;
            GlobalManager.EventSystem.Register<OnLocalPlayerPokemonHealthChange>(OnPokemonHealthChanged);

            PokemonData data = PlayerController.LocalPlayer.pokemonController.data;
            _healthBar.Init(data.currentHealth, 0, data.maxHealth);

            gameObject.SetActive(true);
        }

        public void Close()
        {
            if (PlayerController.LocalPlayer != null)
            {
                PlayerController.LocalPlayer.OnSwitchPokemon -= OnSwitchPokemon;
            }

            GlobalManager.EventSystem.UnRegister<OnLocalPlayerPokemonHealthChange>(OnPokemonHealthChanged);
            gameObject.SetActive(false);
        }


        private void OnPokemonHealthChanged(OnLocalPlayerPokemonHealthChange e)
        {
            _healthBar.Max = e.maxHealth;
            _healthBar.Value = e.currentHealth;
        }

        private void OnSwitchPokemon()
        {
        }
    }
}
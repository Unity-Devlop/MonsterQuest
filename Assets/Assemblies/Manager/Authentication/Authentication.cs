using System;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class Authentication : MonoSingleton<Authentication>
    {
        [SerializeField] private string _playerName;
        [SerializeField] private string _userId;
        [SerializeField] private string _password;
        public static string userId => SingletonNullable._userId;
        public static string playerName => SingletonNullable._playerName;

        protected override bool DontDestroyOnLoad() => false;
    }
}
using System;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class Authentication : MonoSingleton<Authentication>
    {
        [SerializeField] public string _playerName;
        [SerializeField] public string _userId;
        [SerializeField] public string _password;
        public static string userId => SingletonNullable._userId;
        public static string playerName => SingletonNullable._playerName;

        protected override bool DontDestroyOnLoad() => false;
    }
}
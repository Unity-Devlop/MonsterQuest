﻿using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GlobalManager : MonoSingleton<GlobalManager>
    {
        protected override bool DontDestroyOnLoad() => true;
        [field: SerializeField] public Camera camera { get; private set; }
        public LayerMask hittableLayer;
        
    }
}
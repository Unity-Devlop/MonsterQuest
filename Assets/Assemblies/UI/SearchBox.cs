using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class SearchBox : MonoBehaviour
    {
        private TMP_InputField _searchInput;
        private Button _searchButton;
        public event Action<string> OnSearch = delegate { };

        private void Awake()
        {
            _searchButton = transform.Find("SearchButton").GetComponent<Button>();
            _searchInput = transform.Find("SearchInput").GetComponent<TMP_InputField>();
            _searchButton.onClick.AddListener(() => { OnSearch(_searchInput.text); });
        }
    }
}
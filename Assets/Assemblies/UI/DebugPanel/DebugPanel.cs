using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class DebugPanel : MonoSingleton<DebugPanel>
    {
        private TMP_Dropdown itemDropdown;
        // private TextMeshProUGUI numberText;
        private Button _addButton;
        private Dictionary<int, ItemEnum> idx2Enum;
        protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
            gameObject.SetActive(false);
            itemDropdown = transform.Find("AddItem/ItemDropdown").GetComponent<TMP_Dropdown>();
            // numberText = transform.Find("NumberText").GetComponent<TextMeshProUGUI>();
            _addButton = transform.Find("AddItem/AddButton").GetComponent<Button>();
            _addButton.onClick.AddListener(OnAddButtonClicked);
            idx2Enum = new Dictionary<int, ItemEnum>();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (ItemEnum itemEnum in Enum.GetValues(typeof(ItemEnum)))
            {
                idx2Enum.Add(options.Count, itemEnum);
                options.Add(new TMP_Dropdown.OptionData(itemEnum.ToString()));
            }

            itemDropdown.options = options;
        }

        private void OnAddButtonClicked()
        {
            int number = 1;//int.Parse(numberText.text);
            ItemEnum id = idx2Enum[itemDropdown.value];
            Player.Local.HandleAddItem(id, number);
        }

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        public void Close()
        {
            if (Player.Local != null)
            {
                Player.Local.EnableInput();
            }
            gameObject.SetActive(false);
        }

        public void Open()
        {
            if(Player.Local != null)
            {
                Player.Local.DisableInput();
            }
            gameObject.SetActive(true);
        }
    }
}
using TMPro;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class PackageItemDetailPanel : MonoBehaviour, IUISubPanel
    {
        private TextMeshProUGUI _descText;
        private TextMeshProUGUI _nameText;

        private void Awake()
        {
            _nameText = transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            _descText = transform.Find("DescText").GetComponent<TextMeshProUGUI>();
            
            Clear();
        }
        private void Clear()
        {
            _nameText.text = "";
            _descText.text = "";
        }

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        public void Open()
        {
            gameObject.SetActive(true);
            // Debug.Log($"{nameof(PackageItemDetailPanel)} open");
            PackageItemCell.onSelectCellChange += OnSelectCellChange;
        }

        public void Close()
        {
            // Debug.Log($"{nameof(PackageItemDetailPanel)} close");
            PackageItemCell.onSelectCellChange -= OnSelectCellChange;
            Clear();
            gameObject.SetActive(false);
        }

        private void OnSelectCellChange(PackageItemCell obj)
        {
            ItemData data = obj.data;
            _nameText.text = data.name;
            _descText.text = data.desc;
        }
    }
}
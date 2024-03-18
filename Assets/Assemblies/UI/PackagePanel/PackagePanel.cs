using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class PackagePanel : UIPanel
    {
        private Image _typeImage;
        private TextMeshProUGUI _typeText;

        private Button _weaponButton;
        private Button _importantButton;
        private Button _materialButton;
        private Button _propButton;

        private Button _escapeButton;

        private LoopVerticalScrollRect _itemList;
        private EasyGameObjectPool _cellPool;
        private PackageItemDetailPanel _detailPanel;
        private ItemType _currentType;

        private void Awake()
        {
            _currentType = ItemType.宝石;
            RectTransform top = transform.Find("Top") as RectTransform;
            Assert.IsNotNull(top, "Top not found");
            _typeImage = top.Find("TypeImage").GetComponent<Image>();
            _typeText = top.Find("TypeText").GetComponent<TextMeshProUGUI>();
           
            _weaponButton = top.Find("WeaponButton").GetComponent<Button>();
            _importantButton = top.Find("ImportantButton").GetComponent<Button>();
            _materialButton = top.Find("MaterialButton").GetComponent<Button>();
            _propButton = top.Find("PropButton").GetComponent<Button>();

            _weaponButton.onClick.AddListener(OnWeaponButtonClicked);
            _importantButton.onClick.AddListener(OnImportantButtonClicked);
            _materialButton.onClick.AddListener(OnMaterialButtonClicked);
            _propButton.onClick.AddListener(OnPropButtonClicked);


            _itemList = transform.Find("ItemList").GetComponent<LoopVerticalScrollRect>();
            _cellPool = _itemList.GetComponent<EasyGameObjectPool>();

            _itemList.ItemReturn = (tran) => _cellPool.Release(tran.gameObject);
            _itemList.ItemProvider = (idx) => { return _cellPool.Get(); };
            _itemList.itemRenderer = ItemRenderer;

            
            _detailPanel = transform.Find("ItemDetailPanel").GetComponent<PackageItemDetailPanel>();
            
            _escapeButton = transform.Find("EscapeButton").GetComponent<Button>();
            _escapeButton.onClick.AddListener(CloseSelf);

        }

        private void ItemRenderer(Transform tran, int idx)
        {
            PackageItemCell cell = tran.GetComponent<PackageItemCell>();
            ItemData data = Player.Local.package.Get(_currentType, idx);
            cell.SetData(data, idx);
        }

        public override void OnOpened()
        {
            base.OnOpened();
            Player.Local.DisableInput();
            SwitchItemType(_currentType, true);
        }

        public override void OnClosed()
        {
            base.OnClosed();
            if (Player.Local != null)
            {
                Player.Local.EnableInput();
            }
        }

        private void OnPropButtonClicked()
        {
            SwitchItemType(ItemType.道具);
        }

        private void OnMaterialButtonClicked()
        {
            SwitchItemType(ItemType.材料);
        }

        private void OnImportantButtonClicked()
        {
            SwitchItemType(ItemType.重要道具);
        }


        private void OnWeaponButtonClicked()
        {
            SwitchItemType(ItemType.宝石);
        }

        private void SwitchItemType(ItemType type, bool force = false)
        {
            if (_currentType == type && !force)
            {
                return;
            }

            _currentType = type;
            _typeText.text = type.ToString().Substring(0, 2);

            _itemList.totalCount = Player.Local.package.ItemCount(type);
            _itemList.RefillCells();
        }
    }
}
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(LayoutElement))]
    public class PackageItemCell : Selectable, IPointerClickHandler
    {
        private int _idx;

        public ItemData data { get; private set; }
        private TextMeshProUGUI _countText;
        public static PackageItemCell curSelectCell { get; private set; }

        public static Action<PackageItemCell> onSelectCellChange;

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ReSetStatic()
        {
            curSelectCell = null;
            onSelectCellChange = _ => { };
        }
#endif
        protected override void Awake()
        {
            base.Awake();
            _countText = transform.Find("CountText").GetComponent<TextMeshProUGUI>();
        }

        public void SetData(ItemData data, int idx)
        {
            // Debug.Log($"{nameof(PackageItemCell)} SetData {data.count.ToString()}");
            this.data = data;
            _idx = idx;
            _countText.text = data.count.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("OnPointerClick");
            if (curSelectCell != null && curSelectCell == this) return;
            curSelectCell = this;
            onSelectCellChange(this);
        }
        
        public static void ClearSelect()
        {
            curSelectCell = null;
        }
    }
}
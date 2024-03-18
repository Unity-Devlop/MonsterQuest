using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class PackageItemCell : MonoBehaviour, IPointerClickHandler
    {
        private int _idx;
        private ItemData _data;

        public void SetData(ItemData data, int idx)
        {
            this._data = data;
            this._idx = idx;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
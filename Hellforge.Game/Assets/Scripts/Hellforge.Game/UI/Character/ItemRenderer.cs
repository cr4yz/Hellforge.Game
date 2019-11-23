using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {

        [SerializeField]
        private ItemLabelRenderer _itemLabelRenderer;
        [SerializeField]
        private Toggle _equippedToggle;
        [SerializeField]
        private Text _itemNameText;
        [SerializeField]
        private Button _editItemButton;

        private Item _item;

        public void OnPointerEnter(PointerEventData eventData)
        {
            var rt = _itemLabelRenderer.GetComponent<RectTransform>();
            var oldParent = _itemLabelRenderer.transform.parent;
            _itemLabelRenderer.transform.SetParent(transform);
            rt.anchoredPosition = Vector3.zero;
            _itemLabelRenderer.transform.SetParent(oldParent, true);
            _itemLabelRenderer.Render(_item);
            _itemLabelRenderer.gameObject.SetActive(true);
            _itemLabelRenderer.gameObject.RebuildLayout();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var label = FindObjectsOfType<ItemLabelRenderer>()[0];
            label.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Right)
            {
                ToggleEquipped();
            }
        }

        public void Render(Item item)
        {
            _item = item;
            _itemNameText.text = item.BaseName;
            _equippedToggle.isOn = item.Equipped;
            _equippedToggle.onValueChanged.AddListener((bool val) =>
            {
                if (val)
                {
                    item.Equip();
                }
                else
                {
                    item.Unequip();
                }
            });
        }

        private void ToggleEquipped()
        {
            if (_item.Equipped)
            {
                _equippedToggle.isOn = false;
            }
            else
            {
                _equippedToggle.isOn = true;
            }
        }

    }
}


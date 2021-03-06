﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {

        [SerializeField]
        private InventoryRenderer _inventoryRenderer;
        [SerializeField]
        private ItemLabelRenderer _itemLabelRenderer;
        [SerializeField]
        private Toggle _equippedToggle;
        [SerializeField]
        private Text _itemNameText;
        [SerializeField]
        private Button _editItemButton;
        [SerializeField]
        private Button _deleteItemButton;
        [SerializeField]
        private ItemEditorRenderer _itemEditor;

        private Item _item;
        private bool _hovered;

        private void Update()
        {
            var name = _item.ItemName ?? _item.BaseName;
            if (_itemNameText.text != name)
            {
                _itemNameText.text = name;
            }
        }

        private void OnDisable()
        {
            if(_hovered)
            {
                _itemLabelRenderer.gameObject.SetActive(false);
            }
        }

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
            _hovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _itemLabelRenderer.gameObject.SetActive(false);
            _hovered = false;
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
            _editItemButton.onClick.RemoveAllListeners();
            _deleteItemButton.onClick.RemoveAllListeners();
            _equippedToggle.onValueChanged.RemoveAllListeners();

            _item = item;
            _itemNameText.text = item.ItemName ?? item.BaseName;
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

            _editItemButton.onClick.AddListener(() =>
            {
                _itemEditor.gameObject.SetActive(true);
                _itemEditor.Render(item);
            });

            _deleteItemButton.onClick.AddListener(() =>
            {
                _inventoryRenderer.DeleteItem(item);
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


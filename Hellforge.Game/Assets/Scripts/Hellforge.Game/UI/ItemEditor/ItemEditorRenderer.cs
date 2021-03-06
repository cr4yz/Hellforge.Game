﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemEditorRenderer : MonoBehaviour
    {

        [SerializeField]
        private InputField _itemNameInput;
        [SerializeField]
        private Dropdown _itemBaseDropdown;
        [SerializeField]
        private Dropdown _itemAffixDropdown;
        [SerializeField]
        private Button _addAffixButton;
        [SerializeField]
        private Button _cancelButton;
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private ItemAffixRenderer _itemAffixTemplate;

        private List<ItemAffixRenderer> _itemAffixRenderers = new List<ItemAffixRenderer>();

        private void Awake()
        {
            _itemAffixTemplate.gameObject.SetActive(false);
        }

        public void Render(Item item)
        {
            Wipe();

            _itemNameInput.text = item.ItemName ?? item.BaseName;
            _itemBaseDropdown.value = _itemBaseDropdown.options.FindIndex(x => x.text == item.BaseName);
            SetAffixDropdownOptions(item.BaseName);

            foreach(var affix in item.ExplicitAffixes)
            {
                RenderAffix(item, affix);
            }

            _itemBaseDropdown.onValueChanged.AddListener((int value) =>
            {
                var itemBaseName = _itemBaseDropdown.options[value].text;
                item.BaseName = itemBaseName;
                SetAffixDropdownOptions(itemBaseName);
            });

            _itemNameInput.onEndEdit.AddListener((string value) =>
            {
                item.ItemName = value;
            });

            _addAffixButton.onClick.AddListener(() =>
            {
                var affixName = _itemAffixDropdown.options[_itemAffixDropdown.value].text;
                var affix = item.AddExplicitAffix(affixName, 0, 50);
                RenderAffix(item, affix);
            });
        }

        private void SetAffixDropdownOptions(string itemBaseName)
        {
            var itemBase = D4Data.Instance.Hellforge.GameData.ItemBases.First(x => x.Name == itemBaseName);
            _itemAffixDropdown.GetComponent<ItemAffixDropdown>().ItemSlotAffixes(itemBase.Slot);
        }

        private void Wipe()
        {
            _addAffixButton.onClick.RemoveAllListeners();
            _itemNameInput.onEndEdit.RemoveAllListeners();
            _itemBaseDropdown.onValueChanged.RemoveAllListeners();

            foreach (var itemAffixRenderer in _itemAffixRenderers)
            {
                if(itemAffixRenderer != null)
                {
                    GameObject.Destroy(itemAffixRenderer.gameObject);
                }
            }

            _itemAffixRenderers.Clear();
        }

        private void RenderAffix(Item item, ItemAffix affix)
        {
            var clone = GameObject.Instantiate(_itemAffixTemplate, _itemAffixTemplate.transform.parent);
            clone.Render(item, affix);
            clone.gameObject.SetActive(true);
            _itemAffixRenderers.Add(clone);
        }

    }
}


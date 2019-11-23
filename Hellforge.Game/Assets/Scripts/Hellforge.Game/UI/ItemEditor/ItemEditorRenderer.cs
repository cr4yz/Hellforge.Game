﻿using System.Collections.Generic;
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

        private void Start()
        {
            _itemAffixTemplate.gameObject.SetActive(false);
        }

        public void Render(Item item)
        {
            Wipe();

            _itemNameInput.text = item.ItemName ?? item.BaseName;
            _itemBaseDropdown.value = _itemBaseDropdown.options.FindIndex(x => x.text == item.BaseName);

            foreach(var affix in item.ExplicitAffixes)
            {
                RenderAffix(item, affix);
            }

            _itemNameInput.onEndEdit.AddListener((string value) =>
            {
                item.ItemName = value;
            });

            _addAffixButton.onClick.AddListener(() =>
            {
                var afx = new ItemAffix()
                {
                    Name = _itemAffixDropdown.options[_itemAffixDropdown.value].text,
                    Tier = 0,
                    Roll = 50
                };
                item.AddExplicitAffix(afx.Name, afx.Tier, afx.Roll);
                RenderAffix(item, afx);
            });
        }

        private void Wipe()
        {
            _addAffixButton.onClick.RemoveAllListeners();
            
            foreach (var itemAffixRenderer in _itemAffixRenderers)
            {
                GameObject.Destroy(itemAffixRenderer.gameObject);
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


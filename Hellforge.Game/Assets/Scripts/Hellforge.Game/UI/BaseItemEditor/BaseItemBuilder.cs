using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class BaseItemBuilder : MonoBehaviour
    {

        [SerializeField]
        private BaseItemEditor _baseItemEditor;
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _deleteButton;
        [SerializeField]
        private Button _duplicateButton;
        [SerializeField]
        private InputField _identifierInput;
        [SerializeField]
        private Dropdown _typeDropdown;
        [SerializeField]
        private Dropdown _itemSlotDropdown;
        [SerializeField]
        private InputField _iconPathInput;
        [SerializeField]
        private InputField _modelPathInput;
        [SerializeField]
        private InputField _gridWidthInput;
        [SerializeField]
        private InputField _gridHeightInput;
        [SerializeField]
        private InputField _inplicitsText;


        public void Render(ItemBaseEntry baseItem)
        {
            _saveButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
            _duplicateButton.onClick.RemoveAllListeners();

            _saveButton.onClick.AddListener(delegate ()
            {
                Save(baseItem);
                _baseItemEditor.Render();
            });

            _duplicateButton.onClick.AddListener(delegate ()
            {
                var clone = new ItemBaseEntry()
                {
                    Name = baseItem.Name + " (DUPLICATE)",
                    Type = baseItem.Type,
                    InventoryHeight = baseItem.InventoryHeight,
                    InventoryWidth = baseItem.InventoryWidth,
                    Icon = baseItem.Icon,
                    Model = baseItem.Model,
                    Slot = baseItem.Slot
                };
                _baseItemEditor.AddBaseItem(clone);
                Render(clone);
            });

            _deleteButton.onClick.AddListener(delegate ()
            {
                _baseItemEditor.RemoveBaseItem(baseItem);
                gameObject.SetActive(false);
            });

            _identifierInput.text = baseItem.Name;
            _typeDropdown.value = _typeDropdown.options.FindIndex(x => x.text == baseItem.Type);
            _itemSlotDropdown.value = _itemSlotDropdown.options.FindIndex(x => x.text == baseItem.Slot);
            _iconPathInput.text = baseItem.Icon;
            _modelPathInput.text = baseItem.Model;
            _gridWidthInput.text = baseItem.InventoryWidth.ToString();
            _gridHeightInput.text = baseItem.InventoryHeight.ToString();
            //_inplicitsText.text = AffixBuilder.ArrToLsv(baseItem.ImplicitAffixes);
        }

        public void Save(ItemBaseEntry itemEntry)
        {
            itemEntry.Name = _identifierInput.text;
            itemEntry.Type = _typeDropdown.options[_typeDropdown.value].text;
            itemEntry.Slot = _itemSlotDropdown.options[_itemSlotDropdown.value].text;
            itemEntry.Icon = _iconPathInput.text;
            itemEntry.Model = _modelPathInput.text;
            itemEntry.InventoryWidth = int.Parse(_gridWidthInput.text);
            itemEntry.InventoryHeight = int.Parse(_gridHeightInput.text);
        }

    }
}



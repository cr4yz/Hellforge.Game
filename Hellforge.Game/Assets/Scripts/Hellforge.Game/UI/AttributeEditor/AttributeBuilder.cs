using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class AttributeBuilder : MonoBehaviour
    {

        [SerializeField]
        private AttributeEditor _attributeEditor;
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _deleteButton;
        [SerializeField]
        private InputField _identifierInput;
        [SerializeField]
        private Dropdown _categoryDropdown;

        public void Render(AttributeEntry attr)
        {
            _saveButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();

            _identifierInput.text = attr.Name;
            _categoryDropdown.value = _categoryDropdown.options.FindIndex(x => x.text == attr.Category);

            _saveButton.onClick.AddListener(delegate ()
            {
                attr.Name = _identifierInput.text;
                attr.Category = _categoryDropdown.options[_categoryDropdown.value].text;
                _attributeEditor.Render();
            });

            _deleteButton.onClick.AddListener(delegate ()
            {
                _attributeEditor.RemoveAttribute(attr);
                gameObject.SetActive(false);
            });
        }

    }
}


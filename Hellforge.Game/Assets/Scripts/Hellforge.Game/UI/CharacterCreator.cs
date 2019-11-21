using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Entities;
using Hellforge.Core.Serialization;

namespace Hellforge.Game.UI
{
    public class CharacterCreator : MonoBehaviour
    {

        [SerializeField]
        private Dropdown _classDropdown;
        [SerializeField]
        private InputField _nameInput;
        [SerializeField]
        private Button _submitButton;

        void Start()
        {
            _submitButton.onClick.AddListener(SubmitForm);
        }

        void SubmitForm()
        {
            var characterClass = _classDropdown.options[_classDropdown.value].text;
            var characterName = _nameInput.text;

            if(D4Data.Instance.CharacterFiles.ContainsKey(characterName))
            {
                Debug.Log("Character name exists");
                return;
            }

            var character = new Character()
            {
                Class = characterClass,
                Name = characterName
            };

            CharacterSerializer.ToFile(character, D4Data.Instance.CharacterFolderPath + "\\" + characterName + ".d4c");

            D4Data.Instance.RefreshCharacterList();
            GameObject.FindObjectOfType<CharacterSelection>().PopulateCharacterList();

            _nameInput.text = string.Empty;
            _classDropdown.value = 0;
            gameObject.SetActive(false);
        }

    }
}

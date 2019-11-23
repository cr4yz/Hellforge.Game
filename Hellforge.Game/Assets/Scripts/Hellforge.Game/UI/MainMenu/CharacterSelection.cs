using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class CharacterSelection : MonoBehaviour
    {

        [SerializeField]
        private CharacterEntry _characterEntryTemplate;
        private List<CharacterEntry> _characterEntries = new List<CharacterEntry>();

        public void OpenCharacterFolder()
        {
            Application.OpenURL("file://" + D4Data.Instance.CharacterFolderPath);
        }

        public void PopulateCharacterList()
        {
            ClearCharacterList();

            foreach (var character in D4Data.Instance.CharacterFiles)
            {
                var clone = GameObject.Instantiate(_characterEntryTemplate, _characterEntryTemplate.transform.parent);
                clone.SetCharacter(character.Key, character.Value);
                clone.gameObject.SetActive(true);
                clone.CharacterDeleteButton.onClick.AddListener(() =>
                {
                    D4Data.Instance.DeleteCharacterFile(character.Key);
                    PopulateCharacterList();
                });
                clone.CharacterPlayButton.onClick.AddListener(() =>
                {
                    StartGame(character.Key);
                });
                _characterEntries.Add(clone);
            }
        }

        private void Start()
        {
            _characterEntryTemplate.gameObject.SetActive(false);
            PopulateCharacterList();
        }

        private void ClearCharacterList()
        {
            foreach(var entry in _characterEntries)
            {
                GameObject.Destroy(entry.gameObject);
            }
            _characterEntries.Clear();
        }

        private void StartGame(string characterName)
        {
            GameWorld.Instance.EnterWorld(characterName);
        }

    }
}


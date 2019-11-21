using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class CharacterEntry : MonoBehaviour
    {

        [SerializeField]
        private Button _characterSelectButton;
        [SerializeField]
        private Text _characterName;
        [SerializeField]
        public Button CharacterDeleteButton;
        public Button CharacterPlayButton;

        public void SetCharacter(string name, string filePath)
        {
            _characterName.text = name;
        }

    }
}


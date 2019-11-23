using UnityEngine;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class MiscGameWorldUI : MonoBehaviour
    {
        public void SaveCharacter()
        {
            GameWorld.Instance.SaveCharacter();
        }

        public void GoToMainMenu()
        {
            GameWorld.Instance.ExitWorld();
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hellforge.Game
{
    public class Startup : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene("CharacterSelection");
        }
    }
}


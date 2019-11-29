using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Button))]
    public class CommandOnButtonClick : MonoBehaviour
    {
        [SerializeField]
        private string command = "";

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(new UnityEngine.Events.UnityAction(OnClick));
        }

        private void OnClick()
        {
            Console.ExecuteString(command);
        }
    }
}

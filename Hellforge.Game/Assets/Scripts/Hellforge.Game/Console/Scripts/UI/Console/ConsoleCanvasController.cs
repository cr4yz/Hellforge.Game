using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class ConsoleCanvasController : MonoBehaviour
    {

        public static ConsoleCanvasController Instance; 

        /// <summary>
        /// You can reference this in your own scripts to decide if to pause the game, block player movement, or whatever
        /// </summary>
        /// <returns></returns>
        public static bool IsVisible()
        {
            return Instance.isVisible;
        }
        
        [SerializeField]
        private InputField consoleInput;
        [SerializeField]
        private ConsolePanelController consolePanelController;

        private Canvas canvas;
        private GraphicRaycaster graphicRaycaster;

        private int commandsHistoryIndex = 0;
        private List<string> commandsHistory = new List<string>();
        
        private bool isVisible;

        private void Awake()
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            canvas = GetComponent<Canvas>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene newScene, LoadSceneMode mode)
        {
            if (isVisible)
            {
                StartCoroutine(SelectConsoleInputWithDelay());
            }
        }

        private void Start()
        {
            Console.RefreshCommands();
            
            //Hide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) Toggle();
        }

        private IEnumerator SelectConsoleInputWithDelay()
        {
            yield return new WaitForFixedUpdate();

            consoleInput.OnSelect(null);
        }

        private bool AllowOpen()
        {
            /*
             TODO: Insert your own custom logic for deciding if the console can be opened.
             For example, you may choose to only allow the console to open if the current player is a developer, or something.
             
             Example:
             return DeveloperController.IsDeveloper(SteamUser.GetSteamID());
             */

            return true;
        }

        public void Toggle()
        {
            if (isVisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            if (!AllowOpen()) return;

            consolePanelController.ResetCommandHistoryIndex();

            isVisible = true;

            canvas.enabled = isVisible;
            graphicRaycaster.enabled = isVisible;

            StartCoroutine(SelectConsoleInputWithDelay());

            consoleInput.text = "";
        }

        public void Hide()
        {
            isVisible = false;

            canvas.enabled = isVisible;
            graphicRaycaster.enabled = isVisible;

            consoleInput.OnDeselect(null);
        }
    }
}
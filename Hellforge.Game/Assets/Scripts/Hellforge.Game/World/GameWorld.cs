using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hellforge.Core.Entities;
using Hellforge.Core.Serialization;

namespace Hellforge.Game.World
{
    public class GameWorld : SingletonComponent<GameWorld>
    {

        public Character Character { get; private set; }
        private string _loadedLevel;

        public void EnterWorld(string characterName)
        {
            var characterFilePath = D4Data.Instance.CharacterFiles[characterName];
            Character = CharacterSerializer.FromFile(characterFilePath);
            Character.Initialize(D4Data.Instance.Hellforge, new D4Character());

            SceneManager.LoadScene("GameWorld");

            LoadLevel("TestLevel");
        }

        public void ExitWorld()
        {
            Character = null;
            _loadedLevel = null;

            var dontDestry = GameObject.FindObjectsOfType<DontDestroyOnLoad>();
            foreach(var obj in dontDestry)
            {
                if(obj.Identifier == "GameWorld")
                {
                    GameObject.Destroy(obj.gameObject);
                }
            }

            SceneManager.LoadScene("CharacterSelection");
        }

        public void LoadLevel(string levelName, Action<object> onFinishedLoading = null, object context = null)
        {
            var scenesInBuild = GetScenesInBuild();
            if(scenesInBuild.Contains(levelName))
            {
                StartCoroutine(LoadLevelAsync(levelName, onFinishedLoading, context));
            }
        }

        public void SaveCharacter()
        {
            var filePath = D4Data.Instance.CharacterFiles[Character.Name];
            CharacterSerializer.ToFile(Character, filePath);
            UnityEngine.Debug.Log("Character saved!");
        }

        private void Update()
        {
            Character?.Calculate();
        }

        private List<string> GetScenesInBuild()
        {
            var scenesInBuild = new List<string>();
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }
            return scenesInBuild;
        }

        private IEnumerator LoadLevelAsync(string levelName, Action<object> onFinishedLoading = null, object context = null)
        {
            if(_loadedLevel != null)
            {
                var asyncUnload = SceneManager.UnloadSceneAsync(_loadedLevel);
                while (!asyncUnload.isDone)
                    yield return null;
                _loadedLevel = null;
            }

            var asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
            while(!asyncLoad.isDone)
            {
                yield return null;
            }

            _loadedLevel = levelName;

            onFinishedLoading?.Invoke(context);
        }

    }
}

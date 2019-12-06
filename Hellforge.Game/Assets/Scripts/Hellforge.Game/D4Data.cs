using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Core;

namespace Hellforge.Game
{
    public class D4Data : SingletonComponent<D4Data>
    {

        public HellforgeAggregate Hellforge { get; private set; }
        public Dictionary<string, string> CharacterFiles { get; private set; } = new Dictionary<string, string>();

        public string CharacterFolderPath
        {
            get { return Application.persistentDataPath + "/Characters/Diablo4"; }
        }

        private void Awake()
        {
            GameObject.DontDestroyOnLoad(this);

            var dataPath = Application.dataPath + "/RuntimeAssets/HellforgeData/Diablo4";
            Hellforge = new HellforgeAggregate();
            Hellforge.LoadData(dataPath);
            Hellforge.LuaContext.RegisterFunction("print", typeof(D4Data).GetMethod("Print"));

            RefreshCharacterList();
        }

        public void RefreshCharacterList()
        {
            CharacterFiles.Clear();
            Directory.CreateDirectory(CharacterFolderPath);
            var charFiles = Directory.GetFiles(CharacterFolderPath, "*.d4c");
            foreach (var file in charFiles)
            {
                var charName = Path.GetFileNameWithoutExtension(file);
                CharacterFiles.Add(charName, file);
            }
        }

        public void DeleteCharacterFile(string characterName)
        {
            if(!CharacterFiles.ContainsKey(characterName))
            {
                return;
            }
            var path = CharacterFiles[characterName];
            CharacterFiles.Remove(characterName);
            File.Delete(path);
        }

        public static void Print(string message)
        {
            Debug.Log(message);
        }

    }
}

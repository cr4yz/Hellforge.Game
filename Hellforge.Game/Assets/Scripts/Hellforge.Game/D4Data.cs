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

        public override void Setup()
        {
            base.Setup();

            GameObject.DontDestroyOnLoad(this);

            Hellforge = new HellforgeAggregate();
            Hellforge.LoadData(Application.dataPath + "/HellforgeData/Diablo4");

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

    }
}

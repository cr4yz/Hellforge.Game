using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Hellforge.Core.Entities;

namespace Hellforge.Core.Serialization
{
    public struct CharacterInfo
    {
        public CharacterInfo(Character character)
        {
            Name = character.Name;
            Class = character.Class;
            Level = 40;
        }
        public string Name;
        public string Class;
        public int Level;
    }

    public class CharacterSerializer
    {
        public const string Version = "1.0.0";

        public static Character FromFile(string path)
        {
            var lw = new LumpWriter();
            var lumps = lw.ReadFromFile(path);

            var charInfoLump = lumps.Find(x => x.Type == LumpType.CharacterInfo);

            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(charInfoLump.Data))
            {
                return (Character)formatter.Deserialize(ms);
            }

            //var charInfo = ReadCharacterInfo(charInfoLump.Data);
            //var character = new Character(hf, entity);
            //character.Name = charInfo.Name;
            //character.Class = charInfo.Class;
            ////character.Level = charInfo.Level;

            //return character;
        }

        public static void ToFile(Character character, string path)
        {
            // Header
            // Version
            // Items
            // Talents
            // Skills

            var lumps = new List<Lump>()
            {
                new Lump()
                {
                    Data = SerializeCharacterInfo(new CharacterInfo(character), character),
                    Type = LumpType.CharacterInfo
                },
                //new Lump()
                //{
                //    Data = SerializeAllocations(character.Allocations),
                //    Type = LumpType.CharacterAllocations
                //}
            };

            var lw = new LumpWriter();
            lw.WriteToFile(path, Version, lumps);
        }

        // todo: that fancy struct to byte shit nice n eazy
        private static CharacterInfo ReadCharacterInfo(byte[] data)
        {
            var result = new CharacterInfo();
            using (var ms = new MemoryStream(data))
            {
                using (var br = new BinaryReader(ms))
                {
                    result.Name = br.ReadString();
                    result.Class = br.ReadString();
                    result.Level = br.ReadInt32();
                }
            }
            return result;
        }

        private static byte[] SerializeCharacterInfo(CharacterInfo info, Character character = null)
        {
            using (var ms = new MemoryStream())
            {
                var f = new BinaryFormatter();
                f.Serialize(ms, character);
                return ms.ToArray();
                //using (var bw = new BinaryWriter(ms))
                //{
                //    bw.Write(info.Name);
                //    bw.Write(info.Class);
                //    bw.Write(info.Level); // level
                //    return ms.ToArray();
                //}
            }
        }

        private static byte[] SerializeAllocations(CharacterAllocations ca)
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    foreach(var alloc in ca.Points)
                    {
                        bw.Write((int)alloc.Type);
                        bw.Write(alloc.Identifier);
                        bw.Write(alloc.Amount);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
}

using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Hellforge.Core.Serialization
{
    public class LumpWriter
    {
        public void WriteToFile(string filePath, string version, List<Lump> lumps)
        {
            using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(version);
                    bw.Write(lumps.Count);
                    var offset = 0;
                    foreach (var lump in lumps)
                    {
                        bw.Write((int)lump.Type);
                        bw.Write(lump.Data.Length);
                        bw.Write(offset);
                        offset += lump.Data.Length;
                    }
                    bw.Write(fs.Length + 8);
                    foreach (var lump in lumps)
                    {
                        bw.Write(lump.Data);
                    }
                }
            }
        }

        public List<Lump> ReadFromFile(string filePath)
        {
            var lumps = new List<Lump>();
            var allBytes = File.ReadAllBytes(filePath);
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                using (var br = new BinaryReader(fs))
                {
                    var version = br.ReadString();
                    var lumpCount = br.ReadInt32();
                    for (int i = 0; i < lumpCount; i++)
                    {
                        var lump = new Lump()
                        {
                            Type = (LumpType)br.ReadInt32(),
                            Length = br.ReadInt32(),
                            Offset = br.ReadInt32(),
                        };
                        lumps.Add(lump);
                    }
                    var headerOffset = br.ReadInt32();
                    foreach (var l in lumps)
                    {
                        l.Data = allBytes.Skip(headerOffset + l.Offset).Take(l.Length).ToArray();
                    }
                }
            }
            return lumps;
        }

        //private Lump GetHeaderLump(List<Lump> lumps, string version)
        //{
        //    var headerLump = new Lump();
        //    headerLump.Type = LumpType.Header;
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var bw = new BinaryWriter(ms))
        //        {
        //            bw.Write(version);
        //            bw.Write(lumps.Count);
        //            var offset = 0;
        //            foreach (var lump in lumps)
        //            {
        //                bw.Write((int)lump.Type);
        //                bw.Write(lump.Data.Length);
        //                bw.Write(offset);
        //                offset += lump.Data.Length;
        //            }
        //            bw.Write(ms.Length);
        //            headerLump.Data = ms.ToArray();

        //            UnityEngine.Debug.Log("W" + ms.Length);
        //        }
        //    }
        //    return headerLump;
        //}
    }
}
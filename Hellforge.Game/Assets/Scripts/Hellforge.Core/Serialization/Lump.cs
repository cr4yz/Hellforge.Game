
namespace Hellforge.Core.Serialization
{
    public class Lump
    {
        public LumpType Type;
        public int Offset;
        public int Length;
        public int Version;
        public byte[] Data;
    }
}

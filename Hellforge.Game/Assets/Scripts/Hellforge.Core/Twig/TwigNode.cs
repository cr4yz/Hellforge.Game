using Newtonsoft.Json;

namespace Hellforge.Core.Twig
{
    public class TwigNode
    {
        [JsonIgnore]
        public TwigGraph Graph;
        public int Id;
        public float X;
        public float Y;
        public string Type;
        public int Level;
        public int MaxTier;
        public string Affix;
    }
}

using Newtonsoft.Json;

namespace Hellforge.Core.Twig
{
    public class TwigGraph 
    {
        [JsonIgnore]
        public HellforgeAggregate Hellforge;
        public string Class;
        public int Width;
        public int Height;
        public TwigNode[] Nodes;
        public TwigEdge[] Edges;

        public static TwigGraph ParseJson(string json)
        {
            return JsonConvert.DeserializeObject<TwigGraph>(json);
        }
    }
}

using Hellforge.Core.Entities;

namespace Hellforge.Game.World
{
    public class D4Character : IHellforgeEntity
    {
        public object GetContext(string name)
        {
            return "Contextual object";
        }
    }
}


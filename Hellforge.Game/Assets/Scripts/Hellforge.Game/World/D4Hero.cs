using Hellforge.Core.Entities;
using Hellforge.Game.Player;

namespace Hellforge.Game.Entities
{
    public class D4Hero : BaseEntity, IHellforgeEntity
    {

        public PlayerController Controller { get; private set; }

        void Awake()
        {
            Controller = GetComponent<PlayerController>();
        }

        public object GetContext(string name)
        {
            switch(name)
            {
                case "$hero":
                    return this;
            }
            return null;
        }

    }
}


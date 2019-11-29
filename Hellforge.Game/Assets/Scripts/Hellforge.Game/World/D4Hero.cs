using Hellforge.Core;
using Hellforge.Core.Entities;
using Hellforge.Game.Player;
using Hellforge.Game.World;

namespace Hellforge.Game.Entities
{
    public class D4Hero : BaseEntity, IHellforgeEntity
    {

        public Character Character { get; set; }
        public PlayerController Controller { get; private set; }

        void Awake()
        {
            Character = GameWorld.Instance.Character;
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

        private Defense GenerateDefense()
        {
            var result = new Defense();

            foreach(var attr in Character.Attributes)
            {
                switch(attr.Key)
                {
                    case AttributeName.ColdResistance:
                        result.AddDefense(DamageTypeName.Cold, attr.Value);
                        break;
                    case AttributeName.LightningResistance:
                        result.AddDefense(DamageTypeName.Lightning, attr.Value);
                        break;
                    case AttributeName.FireResistance:
                        result.AddDefense(DamageTypeName.Fire, attr.Value);
                        break;
                    case AttributeName.NonPhysicalDamageReduction:
                        result.AddDefense(DamageTypeName.Cold, attr.Value);
                        result.AddDefense(DamageTypeName.Lightning, attr.Value);
                        result.AddDefense(DamageTypeName.Fire, attr.Value);
                        break;
                }
            }

            return result;
        }

    }
}


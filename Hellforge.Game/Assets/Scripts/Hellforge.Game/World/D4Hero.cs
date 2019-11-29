using System.Collections.Generic;
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

        private Queue<DamageInfo> _damageQueue = new Queue<DamageInfo>();

        void Awake()
        {
            Character = GameWorld.Instance.Character;
            Controller = GetComponent<PlayerController>();
        }

        void Update()
        {
            while(_damageQueue.Count > 0)
            {
                var dmgInfo = _damageQueue.Dequeue();
                PostProcessDamage(dmgInfo);
                foreach(IDamageable target in dmgInfo.Targets)
                {
                    target.Damage(dmgInfo);
                }
            }
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

        public void QueueDamage(DamageInfo dmgInfo)
        {
            _damageQueue.Enqueue(dmgInfo);
        }

        public void PostProcessDamage(DamageInfo dmgInfo)
        {
            foreach(var dmg in dmgInfo.Damages)
            {
                switch(dmg.DamageType)
                {
                    case DamageTypeName.Physical:
                        var incPhys = Character.GetAttribute(AttributeName.IncreasedPhysicalDamage);
                        dmg.Amount += dmg.Amount * (incPhys / 100f);
                        break;
                }
            }
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


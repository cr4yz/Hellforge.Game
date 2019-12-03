using System.Collections.Generic;
using Hellforge.Core;

namespace Hellforge.Game.Entities
{
    public class Defense
    {
        private class DamageReduction
        {
            public DamageTypeName DamageType;
            public float Amount;
            public bool Flat;
        }

        private List<DamageReduction> _damageReductions = new List<DamageReduction>();
        public int DefenseAttribute;

        public void ClearDefenses()
        {
            _damageReductions.Clear();
        }

        public void AddDefense(DamageTypeName damageType, float amount, bool flat = false)
        {
            var existing = _damageReductions.Find(x => x.Flat == flat && x.DamageType == damageType);
            if(existing != null)
            {
                existing.Amount += amount;
            }
            else
            {
                _damageReductions.Add(new DamageReduction()
                {
                    Flat = flat,
                    DamageType = damageType,
                    Amount = amount
                });
            }
        }

        public DamageInfo ProcessDamage(DamageInfo dmgInfo)
        {
            var result = new DamageInfo();
            float modifier = 1f;

            if(DefenseAttribute != 0)
            {
                modifier = dmgInfo.AttackAttribute / DefenseAttribute;
            }

            foreach(var dmg in dmgInfo.Damages)
            {
                result.AddDamage(dmg.DamageType, ReduceDamage(dmg));
            }

            return result;
        }

        private float ReduceDamage(Damage dmg, float modifier = 1)
        {
            // todo :this can be better..
            var reduceFlat = 0.0f;
            var flatReduction = _damageReductions.Find(x => x.DamageType == dmg.DamageType && x.Flat);
            if(flatReduction != null)
            {
                reduceFlat = flatReduction.Amount;
            }

            var reducePercent = 0.0f;
            var percentReduction = _damageReductions.Find(x => x.DamageType == dmg.DamageType && !x.Flat);
            if(percentReduction != null)
            {
                reducePercent = percentReduction.Amount;
            }

            var result = dmg.Amount - reduceFlat;
            if(reducePercent != 0)
            {
                result -= dmg.Amount / (100f / reducePercent);
            }

            return result *= modifier;
        }

        public static Defense FromAttributes(Dictionary<AttributeName, float> attributes)
        {
            var result = new Defense();

            foreach (var attr in attributes)
            {
                switch (attr.Key)
                {
                    case AttributeName.Defense:
                        result.DefenseAttribute = (int)attr.Value;
                        break;
                    case AttributeName.PhysicalDamageReduction:
                        result.AddDefense(DamageTypeName.Physical, attr.Value);
                        break;
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


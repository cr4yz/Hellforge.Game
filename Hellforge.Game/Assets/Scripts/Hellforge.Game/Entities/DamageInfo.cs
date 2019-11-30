using System.Collections.Generic;
using Hellforge.Core;

namespace Hellforge.Game.Entities
{
    public class Damage
    {
        public DamageTypeName DamageType;
        public float Min;
        public float Max;
        public float Amount;
    }

    public class DamageInfo
    {

        public AttackTypeName AttackType;
        public List<Damage> Damages { get; private set; } = new List<Damage>();
        public List<BaseEntity> Targets { get; private set; } = new List<BaseEntity>();

        public void AddDamage(DamageTypeName damageType, float amount, float min = 0, float max = 0)
        {
            var existing = Damages.Find(x => x.DamageType == damageType);
            if (existing != null)
            {
                existing.Min += min;
                existing.Max += max;
                existing.Amount += amount;
            }
            else
            {
                Damages.Add(new Damage()
                {
                    DamageType = damageType,
                    Amount = amount,
                    Min = min,
                    Max = max
                });
            }
        }

        public float CalculateTotal()
        {
            var result = 0f;

            foreach(var dmg in Damages)
            {
                result += dmg.Amount;
            }

            return result;
        }

    }
}


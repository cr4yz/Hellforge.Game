using System.Collections.Generic;
using Hellforge.Core;

namespace Hellforge.Game.Entities
{
    public class Damage
    {
        public DamageTypeName DamageType;
        public float Amount;
    }

    public class DamageInfo
    {

        public AttackTypeName AttackType;
        public List<Damage> Damages { get; private set; } = new List<Damage>();

        public void AddDamage(DamageTypeName damageType, float amount)
        {
            var existing = Damages.Find(x => x.DamageType == damageType);
            if (existing != null)
            {
                existing.Amount += amount;
            }
            else
            {
                Damages.Add(new Damage()
                {
                    DamageType = damageType,
                    Amount = amount
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


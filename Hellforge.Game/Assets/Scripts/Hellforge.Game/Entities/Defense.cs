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

            foreach(var dmg in dmgInfo.Damages)
            {
                result.AddDamage(dmg.DamageType, ReduceDamage(dmg));
            }

            return result;
        }

        private float ReduceDamage(Damage dmg)
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

            return result;
        }

    }
}


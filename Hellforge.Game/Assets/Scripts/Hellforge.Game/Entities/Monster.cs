using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Entities
{
    public class Monster : BaseEntity, IDamageable
    {

        public int Health { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;

        protected override void _Start()
        {
            selectable = true;
        }

        public void Damage(DamageInfo dmgInfo)
        {
            var defense = GenerateDefense();
            var finalDamage = defense.ProcessDamage(dmgInfo);
            Health -= (int)finalDamage.CalculateTotal();
            Console.print("Damage me for:" + finalDamage.CalculateTotal());
        }

        private Defense GenerateDefense()
        {
            var result = new Defense();
            result.AddDefense(Core.DamageTypeName.Physical, 10);
            return result;
        }

    }
}

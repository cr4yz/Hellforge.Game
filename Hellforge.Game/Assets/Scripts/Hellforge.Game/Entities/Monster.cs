using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Entities
{
    public class Monster : BaseEntity, IDamageable
    {
        protected override void _Start()
        {
            selectable = true;
        }

        public void Damage(DamageInfo dmgInfo)
        {
            Debug.Log("DAMAGE FOR: " + dmgInfo.Amount);
        }
    }
}


using UnityEngine;

namespace Hellforge.Game.Entities
{
    public class DamageOverTime : BaseBuff
    {

        private DamageInfo _dmgInfo;

        public DamageOverTime(BaseEntity entity, float duration, DamageInfo dmgInfo)
            : base(entity, duration)
        {
            _dmgInfo = dmgInfo;

            foreach(var dmg in _dmgInfo.Damages)
            {
                dmg.Amount *= Time.deltaTime / duration;
            }
        }

        protected override void _OnUpdate()
        {
            if(entity is IDamageable idmg)
            {
                idmg.Damage(_dmgInfo);
            }
        }

    }
}


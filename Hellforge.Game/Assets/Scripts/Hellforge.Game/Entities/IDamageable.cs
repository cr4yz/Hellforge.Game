﻿
namespace Hellforge.Game.Entities
{
    public interface IDamageable
    {
        void Damage(DamageInfo dmgInfo);
        float Health { get; }
        float MaxHealth { get; }
    }
}

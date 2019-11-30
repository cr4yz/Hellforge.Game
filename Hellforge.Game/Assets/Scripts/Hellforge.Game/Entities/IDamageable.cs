
namespace Hellforge.Game.Entities
{
    public interface IDamageable
    {
        void Damage(DamageInfo dmgInfo);
        int Health { get; }
        int MaxHealth { get; }
    }
}

using Hellforge.Core;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class BasicAttack : BaseSkill
    {

        public BasicAttack(D4Hero hero)
            : base(hero, "BasicAttack")
        {
            swingDuration = 0.25f;
            castDuration = 0.1f;
            recoverDuration = 0.25f;
            cooldownDuration = 0f;
            castWithoutTarget = false;
            BlocksInput = true;
        }

        protected override void OnStatusChanged(SkillStatus prevStatus, SkillStatus newStatus)
        {
            if (newStatus == SkillStatus.Swinging)
            {
                hero.Controller.Animator.PlayState("BasicSwing", AnimationTime, true);
            }
        }

        protected override void BeginCast()
        {
            hero.QueueDamage(BuildDamageInfo());
        }

        protected override DamageInfo _BuildDamageInfo()
        {
            var dmgInfo = new DamageInfo();
            dmgInfo.Targets.Add(target);
            dmgInfo.AddDamage(DamageTypeName.Physical, 2, 1, 2);
            return dmgInfo;
        }

    }
}


using Hellforge.Core;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class LungingStrike : BaseSkill
    {

        public LungingStrike(D4Hero hero)
            : base(hero, "LungingStrike")
        {
            BaseRange = 7f;
            swingDuration = 0.65f;
            castDuration = 0.1f;
            recoverDuration = 1f;
            cooldownDuration = 0f;
            castWithoutTarget = false;
            BlocksInput = true;
        }

        protected override void OnStatusChanged(SkillStatus prevStatus, SkillStatus newStatus)
        {
            if (newStatus == SkillStatus.Swinging)
            {
                hero.Controller.AnimateToDestination(destination, 18f);
            }
        }

        protected override void UpdateSwinging()
        {
            if(!hero.Controller.IsMoving)
            {
                ForceStateChange();
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


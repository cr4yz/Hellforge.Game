using Hellforge.Core;
using Hellforge.Game.Entities;
using UnityEngine;

namespace Hellforge.Game.Skills
{
    public class Flay : BaseSkill
    {

        public Flay(D4Hero hero)
            : base(hero, "Flay")
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

            var buffController = target.GetComponent<BuffController>();
            if(buffController != null)
            {
                var rank = hero.Character.Allocations.GetPoints(Core.Entities.AllocationType.Skill, "Flay");
                rank = Mathf.Max(rank - 1, 0);
                var bleedDamage = GetSkillDataValue<int>(rank, "BleedDamage");
                var bleedDuration = GetSkillDataValue<int>(rank, "BleedDuration");
                var dmgInfo = new DamageInfo();
                dmgInfo.AddDamage(DamageTypeName.Bleeding, bleedDamage);
                hero.PostProcessDamage(dmgInfo);
                buffController.AddDamageOverTime(bleedDuration, dmgInfo);
            }
        }

        protected override DamageInfo _BuildDamageInfo()
        {
            var rank = GetSkillRank();
            var minDmg = GetSkillDataValue<int>(rank, "Min");
            var maxDmg = GetSkillDataValue<int>(rank, "Max");
            var final = Random.Range(minDmg, maxDmg + 1);
            var dmgInfo = new DamageInfo();
            dmgInfo.Targets.Add(target);
            dmgInfo.AddDamage(DamageTypeName.Physical, final, minDmg, maxDmg);
            return dmgInfo;
        }

    }
}


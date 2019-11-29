using Hellforge.Core;
using Hellforge.Game.Entities;
using UnityEngine;

namespace Hellforge.Game.Skills
{
    public class Bash : BaseSkill
    {

        public Bash(D4Hero hero)
            : base(hero)
        {
            swingDuration = 0.25f;
            castDuration = 0.1f;
            recoverDuration = 0.25f;
            cooldownDuration = 0f;
            castWithoutTarget = false;
            BlocksInput = true;
        }

        protected override void BeginCast()
        {
            hero.QueueDamage(BuildDamageInfo()); 
        }

        protected override DamageInfo _BuildDamageInfo()
        {
            var rank = hero.Character.Allocations.GetPoints(Core.Entities.AllocationType.Skill, "Bash");
            rank = Mathf.Max(rank - 1, 0);
            var minDmg = GetSkillDataValue<int>("Bash", rank, "Min");
            var maxDmg = GetSkillDataValue<int>("Bash", rank, "Max");
            var final = Random.Range(minDmg, maxDmg + 1);
            var dmgInfo = new DamageInfo();
            dmgInfo.Targets.Add(target);
            dmgInfo.AddDamage(DamageTypeName.Physical, final);
            return dmgInfo;
        }

    }
}


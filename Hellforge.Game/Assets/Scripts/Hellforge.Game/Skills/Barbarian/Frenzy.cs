using System;
using Hellforge.Core;
using Hellforge.Core.Affixes;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class Frenzy : BaseSkill
    {

        public class FrenzyBuff : BaseBuff
        {

            private D4Hero _hero => entity as D4Hero;
            private Affix _frenzyAffix;
            private float _duration;

            private AffixEntry _frenzyAffixData = new AffixEntry()
            {
                Attribute = "AttackSpeed",
                Type = "AttributeModifier",
                Data = new AffixDataEntry[] {
                    new AffixDataEntry() { Minimum = 1, Maximum = 2 }
                }
            };

            public FrenzyBuff(D4Hero hero)
                : base(hero)
            {
                _frenzyAffix = new Affix(_hero.Character, _frenzyAffixData, 0, 0);
                _frenzyAffix.AddNode(new AttributeModifier(_frenzyAffix, false));
            }

            public void Set(int increasedAttackSpeed, int maxAttackSpeed, float duration)
            {
                _frenzyAffix.SetRoll(0);
                _frenzyAffixData.Data[0].Minimum = increasedAttackSpeed;
                _frenzyAffixData.Data[0].Maximum = maxAttackSpeed;
                _duration = duration;
                timer = duration;
            }

            public void Faster()
            {
                _frenzyAffix.Disable();

                var curRoll = _frenzyAffix.Roll;
                var nextRoll = Math.Min(curRoll + 10, 100);
                _frenzyAffix.SetRoll(nextRoll);
                _frenzyAffix.Enable();

                timer = _duration;
            }

            protected override void _OnEnable()
            {
                _frenzyAffix.Enable();
            }

            protected override void _OnDisable()
            {
                _frenzyAffix.Disable();
            }
        }

        private FrenzyBuff _frenzyBuff;

        public Frenzy(D4Hero hero)
            : base(hero, "Frenzy")
        {
            swingDuration = 0.25f;
            castDuration = 0.1f;
            recoverDuration = 0.25f;
            cooldownDuration = 0f;
            castWithoutTarget = false;
            BlocksInput = true;
            _frenzyBuff = new FrenzyBuff(hero);
            hero.GetComponent<BuffController>().AddBuff(_frenzyBuff);
        }

        protected override void OnStatusChanged(SkillStatus prevStatus, SkillStatus newStatus)
        {
            if (newStatus == SkillStatus.Swinging)
            {
                if (_frenzyBuff.State == BaseBuff.BuffState.Active)
                {
                    _frenzyBuff.Faster();
                }
                else
                {
                    if (_frenzyBuff.State == BaseBuff.BuffState.Expired)
                    {
                        _frenzyBuff.Reset();
                    }
                    var rank = GetSkillRank();
                    var incAttackSpeed = GetSkillDataValue<int>(rank, "IncreasedAttackSpeed");
                    var buffDuration = GetSkillDataValue<float>(rank, "IncreasedAttackSpeedDuration");
                    var attackSpeedMax = GetSkillDataValue<int>(rank, "IncreasedAttackSpeedMaximum");
                    _frenzyBuff.Set(incAttackSpeed, attackSpeedMax, buffDuration);
                    _frenzyBuff.Start();
                }

                hero.Controller.Animator.PlayState("BasicSwing", AnimationTime, true);
            }
        }

        protected override void BeginCast()
        {
            hero.QueueDamage(BuildDamageInfo());
        }

        protected override DamageInfo _BuildDamageInfo()
        {
            var rank = GetSkillRank();
            var minDmg = GetSkillDataValue<int>(rank, "Min");
            var maxDmg = GetSkillDataValue<int>(rank, "Max");
            var final = UnityEngine.Random.Range(minDmg, maxDmg + 1);
            var dmgInfo = new DamageInfo();
            dmgInfo.Targets.Add(target);
            dmgInfo.AddDamage(DamageTypeName.Physical, final, minDmg, maxDmg);
            return dmgInfo;
        }

    }
}


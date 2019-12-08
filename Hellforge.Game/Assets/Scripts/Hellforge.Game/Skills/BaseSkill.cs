using System.Linq;
using UnityEngine;
using Hellforge.Game.Entities;
using Newtonsoft.Json.Linq;

namespace Hellforge.Game.Skills
{
    public class BaseSkill 
    {

        protected float swingDuration;
        protected float castDuration;
        protected float recoverDuration;
        protected float cooldownDuration;
        protected bool castWithoutTarget;
        protected D4Hero hero;
        protected BaseEntity target;
        protected Vector3 destination;
        public SkillStatus Status { get; protected set; }
        public bool BlocksInput { get; protected set; }
        public float BaseRange { get; protected set; } = 1f;
        public float AnimationTime => BuffedSwingDuration + recoverDuration + castDuration;
        public bool Queued { get; set; }
        public readonly string SkillName;

        protected float BuffedSwingDuration => ApplyAttackSpeed(swingDuration);
        protected float BuffedCastDuration => ApplyAttackSpeed(castDuration);
        protected float BuffedRecoverDuration => ApplyAttackSpeed(recoverDuration);

        private float _timer;

        protected BaseSkill(D4Hero hero, string skillName)
        {
            this.hero = hero;
            SkillName = skillName;
        }

        public void Update()
        {
            if (Queued)
            {
                if (IsInRange(destination))
                {
                    Cast(target, destination);
                    Queued = false;
                }
            }

            if (Status != SkillStatus.Idle)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0)
                {
                    MoveToNextState();
                }
            }

            switch(Status)
            {
                case SkillStatus.Idle:
                    UpdateIdle();
                    break;
                case SkillStatus.Swinging:
                    UpdateSwinging();
                    break;
                case SkillStatus.Casting:
                    UpdateCasting();
                    break;
                case SkillStatus.Recovering:
                    UpdateRecovering();
                    break;
                case SkillStatus.Cooldown:
                    UpdateCooldown();
                    break;
            }
        }

        private float ApplyAttackSpeed(float amount)
        {
            var attackSpeed = hero.Character.Attributes[Core.AttributeName.AttackSpeed];
            return amount - amount * (attackSpeed / 100f);
        }

        private bool IsInRange(Vector3 destination, bool interactableRange = false)
        {
            var dist = Vector3.Distance(hero.Controller.transform.position, destination);
            var range = interactableRange ? 2f : BaseRange; // + hero.GetAttr[skillRange]...
            return range >= dist;
        }

        public bool Cast(BaseEntity target, Vector3 destination)
        {
            if (!castWithoutTarget && target == null)
            {
                hero.Controller.MoveToDestination(destination);
                return false;
            }

            if (Status != SkillStatus.Idle)
            {
                return false;
            }

            this.target = target;
            this.destination = destination;

            if (!IsInRange(destination))
            {
                Queued = true;
                hero.Controller.MoveToDestination(destination);
                return false;
            }

            // check available resources...
            hero.Controller.StopMoving();

            if (target is IDamageable)
            {
                MoveToNextState();
                return true;
            }
            else if(target is IInteractable ii)
            {
                ii.Interact();
                return false;
            }

            return false;
        }

        public bool IsBusy()
        {
            if(Status == SkillStatus.Swinging 
                || Status == SkillStatus.Casting)
            {
                return true;
            }
            // improve character responsiveness
            if (Status == SkillStatus.Recovering)
            {
                return _timer >= recoverDuration / 2f;
            }

            return false;
        }

        public DamageInfo BuildDamageInfo(bool excludeTargets = false)
        {
            var result = _BuildDamageInfo();
            if (excludeTargets)
            {
                result.Targets.Clear();
            }
            return result;
        }

        protected void ForceStateChange()
        {
            _timer = 0;
        }

        protected virtual void UpdateIdle() { }
        protected virtual void UpdateSwinging() { }
        protected virtual void UpdateCasting() { }
        protected virtual void UpdateRecovering() { }
        protected virtual void UpdateCooldown() { }
        protected virtual void BeginCast() { }
        protected virtual void OnStatusChanged(SkillStatus prevStatus, SkillStatus newStatus) { }
        protected virtual DamageInfo _BuildDamageInfo() { return new DamageInfo(); }

        private void MoveToNextState()
        {
            var oldStatus = Status;

            switch(Status)
            {
                case SkillStatus.Idle:
                    _timer = BuffedSwingDuration;
                    Status = SkillStatus.Swinging;
                    break;
                case SkillStatus.Swinging:
                    _timer = castDuration;
                    Status = SkillStatus.Casting;
                    BeginCast();
                    break;
                case SkillStatus.Casting:
                    _timer = recoverDuration;
                    Status = SkillStatus.Recovering;
                    break;
                case SkillStatus.Recovering:
                    _timer = cooldownDuration;
                    Status = SkillStatus.Cooldown;
                    break;
                case SkillStatus.Cooldown:
                    _timer = 0;
                    Status = SkillStatus.Idle;
                    break;
            }

            OnStatusChanged(oldStatus, Status);
        }

        public T GetSkillDataValue<T>(int rank, string dataKey)
        {
            var skillEntry = hero.Character.Hellforge.GameData.Skills.First(x => x.Name == SkillName);
            if(skillEntry == null)
            {
                return default;
            }

            var rankObj = (JObject)skillEntry.Data[rank];
            if(rankObj.TryGetValue(dataKey, System.StringComparison.InvariantCultureIgnoreCase, out JToken value))
            {
                return value.Value<T>();
            }

            return default;
        }

        public int GetSkillRank()
        {
            var rank = hero.Character.Allocations.GetPoints(Core.Entities.AllocationType.Skill, SkillName);
            return Mathf.Max(rank - 1, 0);
        }

    }
}


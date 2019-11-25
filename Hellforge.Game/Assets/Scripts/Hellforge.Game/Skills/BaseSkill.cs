﻿using UnityEngine;
using Hellforge.Game.World;
using Hellforge.Game.Entities;

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
        protected IInteractable target;
        protected Vector3 destination;
        public SkillStatus Status { get; protected set; }
        public bool BlocksInput { get; protected set; }
        public float BaseRange { get; protected set; } = 1f;
        public bool Queued { get; set; }

        private float _timer;

        protected BaseSkill(D4Hero hero)
        {
            this.hero = hero;
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

        private bool IsInRange(Vector3 destination)
        {
            var dist = Vector3.Distance(hero.Controller.transform.position, destination);
            var range = BaseRange; // + hero.GetAttr[skillRange]...
            return range >= dist;
        }

        public bool Cast(IInteractable target, Vector3 destination)
        {
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

            if (!castWithoutTarget && target == null)
            {
                hero.Controller.MoveToDestination(destination);
                return false;
            }

            // check available resources...
            hero.Controller.StopMoving();
            MoveToNextState();

            return true;
        }

        public bool IsBusy()
        {
            if(Status == SkillStatus.Swinging 
                || Status == SkillStatus.Casting 
                || Status == SkillStatus.Recovering)
            {
                return true;
            }
            return false;
        }

        protected virtual void UpdateIdle() { }
        protected virtual void UpdateSwinging() { }
        protected virtual void UpdateCasting() { }
        protected virtual void UpdateRecovering() { }
        protected virtual void UpdateCooldown() { }
        protected virtual void BeginCast() { }
        protected virtual void OnStatusChanged(SkillStatus prevStatus, SkillStatus newStatus) { }

        private void MoveToNextState()
        {
            var oldStatus = Status;

            switch(Status)
            {
                case SkillStatus.Idle:
                    _timer = swingDuration;
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

    }
}

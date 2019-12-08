using UnityEngine;

namespace Hellforge.Game.Entities
{
    public abstract class BaseBuff
    {

        public enum BuffState
        {
            Idle,
            Active,
            Expired
        }

        public BaseBuff(BaseEntity entity, float timer = 0f)
        {
            this.entity = entity;
            this.timer = timer;
            _originalTimer = timer;
        }

        public BuffState State { get; private set; }
        protected BaseEntity entity;
        protected float timer;
        private float _originalTimer;

        public void Update()
        {
            if(State != BuffState.Active)
            {
                return;
            }

            if(timer > 0)
            {
                timer -= Time.deltaTime;
                if(timer <= 0)
                {
                    Stop();
                    return;
                }
            }

            _OnUpdate();
        }

        public void Reset()
        {
            if(State == BuffState.Active)
            {
                Stop();
            }
            State = BuffState.Idle;
            timer = _originalTimer;
        }

        public void Start()
        {
            if(State != BuffState.Idle)
            {
                return;
            }

            State = BuffState.Active;
            _OnEnable();
        }

        public void Stop()
        {
            if(State != BuffState.Active)
            {
                return;
            }

            State = BuffState.Expired;
            _OnDisable();
        }

        protected virtual void _OnUpdate() { }
        protected virtual void _OnEnable() { }
        protected virtual void _OnDisable() { }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Entities
{
    public abstract class BaseBuff
    {

        public BaseBuff(BaseEntity entity, float timer = 0f)
        {
            this.entity = entity;
            _timer = timer;
        }

        protected BaseEntity entity;
        public bool Expired { get; private set; }
        private float _timer;
        private bool _enabled;

        public void Update()
        {
            if(Expired)
            {
                return;
            }

            if(!_enabled)
            {
                _OnEnable();
                _enabled = true;
            }

            if(_timer > 0)
            {
                _timer -= Time.deltaTime;
                if(_timer <= 0)
                {
                    Expired = true;
                    _OnDisable();
                    return;
                }
            }

            _OnUpdate();
        }

        protected virtual void _OnUpdate() { }
        protected virtual void _OnEnable() { }
        protected virtual void _OnDisable() { }

    }
}


using System;
using UnityEngine;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public abstract class PhysicalQuery : MonoBehaviour
    {
        public Action<BaseEntity> OnEntityHit;

        public void Complete()
        {
            OnEntityHit = null;
            GameObject.Destroy(gameObject);
        }

        protected void HitDetected(BaseEntity entity)
        {
            OnEntityHit?.Invoke(entity);
        }
    }
}


﻿using System.Collections.Generic;
using UnityEngine;
using Hellforge.Core;
using Hellforge.Core.Entities;
using Hellforge.Game.Player;
using Hellforge.Game.World;

namespace Hellforge.Game.Entities
{
    public class D4Hero : BaseEntity, IHellforgeEntity
    {

        public Character Character => GameWorld.Instance.Character;
        public PlayerController Controller { get; private set; }

        public Vector3 Position => transform.position;
        public Quaternion Rotation => GetComponent<PlayerAnimator>().Rotation;

        private Queue<DamageInfo> _damageQueue = new Queue<DamageInfo>();

        void Awake()
        {
            gameObject.GetOrAddComponent<BuffController>();
            Controller = GetComponent<PlayerController>();
        }

        void Update()
        {
            while(_damageQueue.Count > 0)
            {
                var dmgInfo = _damageQueue.Dequeue();
                PostProcessDamage(dmgInfo);
                foreach (IDamageable target in dmgInfo.Targets)
                {
                    target.Damage(dmgInfo);
                }
            }
        }

        public object GetContext(string name)
        {
            switch(name)
            {
                case "$hero":
                    return this;
            }
            return null;
        }

        public void QueueDamage(DamageInfo dmgInfo)
        {
            _damageQueue.Enqueue(dmgInfo);
        }

        public void PostProcessDamage(DamageInfo dmgInfo)
        {
            dmgInfo.AttackAttribute = (int)Character.GetAttribute(AttributeName.Attack);

            foreach(var dmg in dmgInfo.Damages)
            {
                switch(dmg.DamageType)
                {
                    case DamageTypeName.Physical:
                        var incPhys = Character.GetAttribute(AttributeName.IncreasedPhysicalDamage);
                        dmg.Amount += dmg.Amount * (incPhys / 100f);
                        dmg.Min += dmg.Min * (incPhys / 100f);
                        dmg.Max += dmg.Max * (incPhys / 100f);
                        break;
                    case DamageTypeName.Bleeding:
                        var incBleed = Character.GetAttribute(AttributeName.MoreBleedingDamage);
                        dmg.Amount += dmg.Amount * (incBleed / 100f);
                        dmg.Min += dmg.Min * (incBleed / 100f);
                        dmg.Max += dmg.Max * (incBleed / 100f);
                        break;
                }
            }
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class DoubleSwing : BaseSkill
    {

        public DoubleSwing(D4Hero hero)
            : base(hero, "DoubleSwing")
        {
            swingDuration = 0.45f;
            castDuration = 0.1f;
            recoverDuration = 0.55f;
            cooldownDuration = 0f;
            BaseRange = 1.5f;
            castWithoutTarget = true;
            BlocksInput = true;
        }

        protected override void BeginCast()
        {
            var pqResource = Resources.Load<GameObject>("Diablo4/Prefabs/PhysicalQueries/DoubleSwing");
            var pq = GameObject.Instantiate<GameObject>(pqResource).GetComponent<PQDoubleSwing>();
            pq.Begin(hero.Position + Vector3.up, hero.Rotation);
        }

    }
}


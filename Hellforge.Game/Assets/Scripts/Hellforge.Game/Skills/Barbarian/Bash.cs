using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.Entities;

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
            Debug.Log("BASH");
        }

    }
}


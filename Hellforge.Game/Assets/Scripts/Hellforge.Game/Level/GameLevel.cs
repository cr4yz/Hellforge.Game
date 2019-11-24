using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.World;

namespace Hellforge.Game.Level
{
    public class GameLevel : SingletonComponent<GameLevel>
    {

        void Start()
        {
            GameWorld.Instance.Hero.Spawn();
        }

    }
}


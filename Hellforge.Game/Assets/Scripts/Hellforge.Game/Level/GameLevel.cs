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
            var d4c = GameWorld.Instance.Character.Entity as D4Character;
            d4c.Spawn();
        }

    }
}


using UnityEngine;
using Hellforge.Core.Entities;
using Hellforge.Game.Player;
using Hellforge.Game.Skills;

namespace Hellforge.Game.World
{
    public class D4Hero : IHellforgeEntity
    {

        public PlayerController Controller { get; private set; }

        public void Spawn()
        {
            var spawnPos = GameObject.Find("SPAWN").transform.position;
            var playerPrefab = GameObject.Instantiate(Resources.Load("Diablo4/Prefabs/Player")) as GameObject;
            Controller = playerPrefab.GetComponent<PlayerController>();
            playerPrefab.transform.position = spawnPos;

            // todo: save/load skill assignments
            Controller.SlotSkill(SkillSlot.Primary, new Bash(this));
        }

        public object GetContext(string name)
        {
            switch(name)
            {
                case "$character":
                    return this;
            }
            return null;
        }

    }
}


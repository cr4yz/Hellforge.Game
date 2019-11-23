using UnityEngine;
using Hellforge.Core.Entities;
using Hellforge.Game.Player;

namespace Hellforge.Game.World
{
    public class D4Character : IHellforgeEntity
    {

        public PlayerController Controller { get; private set; }
        public Character HellforgeCharacter => GameWorld.Instance.Character;

        public void Spawn()
        {
            var spawnPos = GameObject.Find("SPAWN").transform.position;
            var playerPrefab = GameObject.Instantiate(Resources.Load("Diablo4/Prefabs/Player")) as GameObject;
            Controller = playerPrefab.GetComponent<PlayerController>();
            playerPrefab.transform.position = spawnPos;
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


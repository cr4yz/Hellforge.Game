using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Core;
using Hellforge.Game.World;

namespace Hellforge.Game.Entities
{
    public class Monster : BaseEntity, IDamageable
    {

        private GameObject _monsterPrefab;
        [SerializeField]
        private string _monsterName;
        public int Health { get; private set; } = 50;
        public int MaxHealth { get; private set; } = 50;
        public int MonsterLevel { get; private set; }
        public Dictionary<AttributeName, float> Attributes { get; } = new Dictionary<AttributeName, float>();

        protected override void _Start()
        {
            selectable = true;

            if(_monsterName != null)
            {
                AssignMonster(_monsterName);
            }
        }

        public void AssignMonster(string monsterName, int monsterLevel = 1)
        {
            var monster = D4Data.Instance.Hellforge.GameData.Monsters.FirstOrDefault(x => x.Name == monsterName);
            if(monster == null)
            {
                Console.print("Monster " + monsterName + " doesn't exist");
                return;
            }

            Attributes.Clear();
            Health = monster.Health;
            MaxHealth = Health;
            DisplayName = monster.Name;

            foreach(var attr in monster.Attributes)
            {
                if(Enum.TryParse(attr.Key, out AttributeName attributeName))
                {
                    Attributes.Add(attributeName, attr.Value);
                }
            }

            SetMonsterLevel(monsterLevel);
        }

        public void SetMonsterLevel(int level)
        {
            if(Attributes.ContainsKey(AttributeName.Attack))
            {
                Attributes[AttributeName.Attack] += 50 * level;
            }
            if (Attributes.ContainsKey(AttributeName.Defense))
            {
                Attributes[AttributeName.Defense] += 50 * level;
            }
        }

        public void Damage(DamageInfo dmgInfo)
        {
            var defense = Defense.FromAttributes(Attributes);
            var finalDamage = defense.ProcessDamage(dmgInfo);
            Health -= (int)finalDamage.CalculateTotal();

            if(Health <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }

        public static Monster Spawn(string monsterName, int monsterLevel, Vector3 position)
        {
            var baseMonster = GameObject.Instantiate(Resources.Load<GameObject>("Diablo4/Prefabs/BaseMonster"));
            baseMonster.transform.position = position;
            var monster = baseMonster.AddComponent<Monster>();

            monster.AssignMonster(monsterName, monsterLevel);

            return monster;
        }

        [ConCommand("monster_spawn", "Spawns a monster where your cursor is")]
        public static void MonsterSpawn(string monsterName, int monsterLevel = 1)
        {
            var hitPoint = GameWorld.Instance.Hero.Controller.GetMouseHitPoint();
            Spawn(monsterName, monsterLevel, hitPoint);
        }

    }
}

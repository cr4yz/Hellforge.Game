using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Hellforge.Core.Affixes;
using Hellforge.Core.Entities;

namespace Hellforge.Core.Items
{
    [Serializable]
    public class ItemAffix
    {
        public string Name;
        public int Tier;
        public int Roll;
    }

    [Serializable]
    public class Item : ISerializable
    {

        public string ItemName;
        public readonly string BaseName;
        public readonly int BaseRoll;
        public readonly int ItemLevel;
        public bool Equipped { get; private set; }
        public Character Character { get; private set; }
        public List<ItemAffix> ExplicitAffixes { get; private set; } = new List<ItemAffix>();
        private List<Affix> _equippedAffixes = new List<Affix>();
        private bool _equipApplied;

        public Item(string baseName, int iLvl, int roll)
        {
            BaseName = baseName;
            BaseRoll = roll;
            ItemLevel = iLvl;
        }

        // todo :This init shit is dumb.. isnt' it
        // maybe character reference should be set in a constructor??
        // need to refactor to organize that
        public void Initialize(Character character)
        {
            if(Character != null)
            {
                throw new Exception("Don't initialize item twice.");
            }
            Character = character;
        }

        public void Equip()
        {
            if (_equipApplied)
            {
                throw new Exception("That item is already equipped");
            }

            if(Character == null)
            {
                throw new Exception("That item isn't initialized yet");
            }

            Equipped = true;
            _equipApplied = true;

            var itemData = Character.Hellforge.GameData.ItemBases.FirstOrDefault(x => x.Name == BaseName);
            foreach(var implict in itemData.ImplicitAffixes)
            {
                _equippedAffixes.Add(Character.AddAffix(implict.Name, implict.Tier, BaseRoll));
            }

            foreach(var explict in ExplicitAffixes)
            {
                _equippedAffixes.Add(Character.AddAffix(explict.Name, explict.Tier, explict.Roll));
            }

            Character.Dirty = true;
        }

        public void Unequip()
        {
            if(!_equipApplied)
            {
                throw new Exception("That item isn't equipped");
            }

            if (Character == null)
            {
                throw new Exception("That item isn't initialized yet");
            }

            Equipped = false;
            _equipApplied = false;

            foreach (var affix in _equippedAffixes)
            {
                Character.RemoveAffix(affix);
            }

            Character.Dirty = true;
        }

        public ItemAffix AddExplicitAffix(string name, int tier, int roll)
        {
            var affix = new ItemAffix()
            {
                Name = name,
                Tier = tier,
                Roll = roll
            };
            ExplicitAffixes.Add(affix);
            Character.Dirty = true;
            return affix;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ItemName", ItemName);
            info.AddValue("BaseName", BaseName);
            info.AddValue("BaseRoll", BaseRoll);
            info.AddValue("ItemLevel", ItemLevel);
            info.AddValue("Equipped", Equipped);
            info.AddValue("ExplicitAffixes", ExplicitAffixes);
        }

        public Item(SerializationInfo info, StreamingContext context)
        {
            foreach (SerializationEntry entry in info)
            {
                switch (entry.Name)
                {
                    case "ItemName":
                        ItemName = (string)entry.Value;
                        break;
                    case "BaseName":
                        BaseName = (string)entry.Value;
                        break;
                    case "BaseRoll":
                        BaseRoll = (int)entry.Value;
                        break;
                    case "ItemLevel":
                        ItemLevel = (int)entry.Value;
                        break;
                    case "Equipped":
                        Equipped = (bool)entry.Value;
                        break;
                    case "ExplicitAffixes":
                        ExplicitAffixes = (List<ItemAffix>)entry.Value;
                        break;
                }
            }
        }

    }
}


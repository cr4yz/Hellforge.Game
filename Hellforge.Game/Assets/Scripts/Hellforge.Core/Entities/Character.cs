using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Hellforge.Core.Affixes;
using Hellforge.Core.Items;

namespace Hellforge.Core.Entities
{
    public delegate void EmptyEventHandler();

    [Serializable]
    public class Character : ISerializable
    {

        public event EmptyEventHandler OnChanged;

        public string Name;
        public string Class;
        public int Level;
        public IHellforgeEntity Entity { get; set; }
        public HellforgeAggregate Hellforge { get; private set; }
        public CharacterAllocations Allocations { get; private set; }
        public List<Affix> Affixes { get; private set; } = new List<Affix>();
        public List<Item> Items { get; private set; } = new List<Item>();
        public Dictionary<AttributeName, float> Attributes { get; } = new Dictionary<AttributeName, float>();
        public bool Dirty;
        private bool _initialized;

        public Character() { Allocations = new CharacterAllocations(this); }

        public void Initialize(HellforgeAggregate hellforge, IHellforgeEntity entity = null)
        {
            // todo :This init shit is dumb.. isnt' it
            if(_initialized)
            {
                throw new Exception("Don't initialize character twice");
            }

            Hellforge = hellforge;
            Entity = entity;

            foreach(AttributeName attr in Enum.GetValues(typeof(AttributeName)))
            {
                Attributes.Add(attr, 0);
            }

            foreach (var point in Allocations.Points)
            {
                if(point.Type == AllocationType.Talent)
                {
                    AddAffix(point.Identifier, point.Amount, 100);
                }
            }

            foreach (var item in Items)
            {
                item.Initialize(this);

                if (item.Equipped)
                {
                    item.Equip();
                }
            }

            _initialized = true;
        }

        public void AddItem(Item item)
        {
            if (Items.Contains(item))
            {
                throw new Exception("item is already added");
            }

            Items.Add(item);
            item.Initialize(this);
            Dirty = true;
        }

        public void RemoveItem(Item item)
        {

            if(!Items.Contains(item))
            {
                throw new Exception("item doesn't exist in that character's inventory");
            }

            if (item.Equipped)
            {
                item.Unequip();
            }
            Items.Remove(item);
            Dirty = true;
        }

        public void Update()
        {
            foreach(var item in Items)
            {
                if(item.Dirty && item.Equipped)
                {
                    item.Unequip();
                    item.Equip();
                }
            }

            foreach (var affix in Affixes)
            {
                affix.Update();
            }

            if (Dirty)
            {
                Dirty = false;
                OnChanged?.Invoke();
            }
        }

        public float GetAttribute(string attributeName)
        {
            if(Enum.TryParse(attributeName, out AttributeName attrName))
            {
                return GetAttribute(attrName);
            }
            return 0;
        }

        public float GetAttribute(AttributeName attributeName)
        {
            if(Attributes.ContainsKey(attributeName))
                return Attributes[attributeName];
            return 0f;
        }

        public int GetAffixTier(string affixName)
        {
            var affix = Affixes.Find(x => x.AffixData.Name == affixName);
            if(affix != null)
            {
                return affix.Tier;
            }
            return 0;
        }

        public Affix GetAffix(string affixName)
        {
            return Affixes.Find(x => x.AffixData.Name == affixName);
        }

        public Affix AddAffix(string affixName, int tier = 0, int roll = 0)
        {
            var affix = Hellforge.GenerateAffix(this, affixName, tier, roll);
            if(affix == null)
            {
                return null;
            }

            Affixes.Add(affix);

            Dirty = true;

            return affix;
        }

        public void RemoveAffix(Affix affix)
        {
            if(!Affixes.Contains(affix))
            {
                return;
            }

            affix.Disable();
            Affixes.Remove(affix);
            Dirty = true;
        }

        public Affix UpdateAffix(string affixName, int tier)
        {
            var multiple = Affixes.Count(x => x.AffixData.Name == affixName);
            if(multiple > 1)
            {
                throw new Exception("Don't use string identifier for common affixes sharing a name");
            }

            RemoveAffix(affixName);
            Dirty = true;

            return AddAffix(affixName, tier);
        }

        public void RemoveAffix(string affixName)
        {
            var multiple = Affixes.Count(x => x.AffixData.Name == affixName);
            if (multiple > 1)
            {
                throw new Exception("Don't use string identifier for common affixes sharing a name");
            }

            var affix = GetAffix(affixName);
            if(affix == null)
            {
                return;
            }

            affix.Disable();
            Affixes.Remove(affix);
            Dirty = true;
        }

        public void Activate(string eventName)
        {
            var affixes = Affixes.FindAll(x => x.AffixData.Activators != null && x.AffixData.Activators.Contains(eventName));
            foreach(var affix in Affixes)
            {
                affix.Activate(); 
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Class", Class);
            info.AddValue("Level", Level);
            info.AddValue("Allocations", Allocations.Points);
            info.AddValue("Items", Items);
        }

        public Character(SerializationInfo info, StreamingContext context)
        {
            Allocations = new CharacterAllocations(this);

            foreach(SerializationEntry entry in info)
            {
                switch(entry.Name)
                {
                    case "Name":
                        Name = (string)entry.Value;
                        break;
                    case "Class":
                        Class = (string)entry.Value;
                        break;
                    case "Level":
                        Level = (int)entry.Value;
                        break;
                    case "Allocations":
                        Allocations.Points = (List<Allocation>)entry.Value;
                        break;
                    case "Items":
                        Items = (List<Item>)entry.Value;
                        break;
                }
            }
        }

    }
}

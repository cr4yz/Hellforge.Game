using System;
using System.Linq;
using System.Collections.Generic;

namespace Hellforge.Core.Entities
{
    public enum AllocationType
    {
        None = 0,
        Talent = 1,
        Skill = 2
    }

    [Serializable]
    public class Allocation
    {
        public AllocationType Type;
        public string Identifier;
        public int Amount;
    }

    public class CharacterAllocations
    {
        public List<Allocation> Points { get; set; } = new List<Allocation>();

        public int GetPoints(AllocationType type, string identifier)
        {
            var allocation = Points.Find(x => x.Type == type && x.Identifier == identifier);
            if(allocation != null)
            {
                return allocation.Amount;
            }
            return 0;
        }

        public void SetAllocation(AllocationType type, string identifier, int amount)
        {
            var allocation = Points.Find(x => x.Identifier == identifier);
            if (allocation != null)
            {
                allocation.Amount = amount;
            }
            else
            {
                Points.Add(new Allocation()
                {
                    Type = type,
                    Identifier = identifier,
                    Amount = amount
                });
            }
        }

        public void IncrementAllocation(AllocationType type, string identifier, int amount)
        {
            var allocation = Points.Find(x => x.Identifier == identifier);
            if(allocation != null)
            {
                allocation.Amount += amount;
            }
            else
            {
                Points.Add(new Allocation()
                {
                    Type = type,
                    Identifier = identifier,
                    Amount = amount
                });
            }
        }

        public void RemoveAllocation(string identifier)
        {
            var allocation = Points.Find(x => x.Identifier == identifier);
            if(allocation != null)
            {
                Points.Remove(allocation);
            }
        }
    }
}
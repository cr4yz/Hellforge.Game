using System;
using System.Linq;
using System.Collections.Generic;
using Hellforge.Core.Entities;

namespace Hellforge.Core.Affixes
{
    public enum AffixType
    {
        None,
        AttributeModifier,
        Logic
    }

    public enum AffixStatus
    {
        Disabled,
        Enabled
    }

    public class Affix
    {
        public List<AffixNode> Nodes = new List<AffixNode>();
        public readonly Character Character;
        public int Tier { get; private set; }
        public int Roll { get; private set; }
        public readonly int TierCount;
        public readonly AffixEntry AffixData;
        private AffixStatus _status;
        //public int TierDataIndex => Math.Max(Tier - 1, 0);
        public int TierDataIndex => Tier;

        public Affix(Character character, AffixEntry affixData, int tier = 0, int roll = 100)
        {
            Character = character;
            AffixData = affixData;
            Tier = tier;
            TierCount = affixData.Data.Length;
            Roll = roll;
        }

        public Affix AddNode(AffixNode node)
        {
            Nodes.Add(node);
            return this;
        }

        public void Activate()
        {
            foreach (var node in Nodes)
            {
                node.Activate();
            }
        }

        public void Disable()
        {
            if(_status == AffixStatus.Disabled)
            {
                return;
            }
            foreach(var node in Nodes)
            {
                node.Disable();
            }
        }

        public void SetTier(int value)
        {
            if(value < 0)
            {
                value = 0;
            }
            Tier = Math.Min(TierCount, value);
            Disable();
        }

        public void SetRoll(int value)
        {
            Roll = value;
            Disable();
        }

        public void Update()
        {
            var conditions = Nodes.FindAll(x => x is ScriptedCondition);
            foreach(var condition in conditions)
            {
                if(!((ScriptedCondition)condition).Passes())
                {
                    if(_status == AffixStatus.Enabled)
                    {
                        foreach(var node in Nodes)
                        {
                            node.Disable();
                        }
                        _status = AffixStatus.Disabled;
                    }
                    return;
                }
            }

            if(_status == AffixStatus.Disabled)
            {
                foreach(var node in Nodes)
                {
                    node.Enable();
                }
                _status = AffixStatus.Enabled;
            }

            foreach(var node in Nodes)
            {
                node.Update();
            }
        }

    }
}

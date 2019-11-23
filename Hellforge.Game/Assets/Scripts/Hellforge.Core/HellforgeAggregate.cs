using System;
using System.Linq;
using System.IO;
using NLua;
using Newtonsoft.Json.Linq;
using Hellforge.Core.Affixes;
using Hellforge.Core.Entities;
using Hellforge.Core.Twig;

namespace Hellforge.Core
{
    public class HellforgeAggregate
    {

        public Lua LuaContext { get; private set; }
        public GameDataObject GameData { get; private set; }

        public void LoadData(string dataDirectory)
        {
            LuaContext?.Dispose();
            LuaContext = new Lua();
            var dataSource = new JObject();

            foreach (string file in Directory.EnumerateFiles(dataDirectory, "*.*", SearchOption.AllDirectories))
            {
                var ext = Path.GetExtension(file);
                if(ext == ".lua")
                {
                    LuaContext.DoFile(file);
                }
                else if(ext == ".json")
                {
                    var text = File.ReadAllText(file);
                    dataSource.Merge(JObject.Parse(text));
                }
            }

            GameData = dataSource.ToObject<GameDataObject>();

            foreach(var skillTree in GameData.SkillTrees)
            {
                skillTree.Hellforge = this;
                foreach(var node in skillTree.Nodes)
                {
                    node.Graph = skillTree;
                }
            }
        }

        public int GetAffixTierCount(string affix)
        {
            var affixData = GameData.Affixes.First(x => x.Name == affix);
            if(affixData != null && affixData.Data != null)
            {
                return affixData.Data.Length;
            }
            return 0;
        }

        public Affix GenerateAffix(Character character, string name, int tier = 0, int roll = 0)
        {
            var affixData = GameData.Affixes.First(x => x.Name == name);

            if(affixData == null)
            {
                return null;
            }

            if(affixData.Inherits != null)
            {
                var parent = GameData.Affixes.First(x => x.Name == affixData.Inherits);
                if(parent != null)
                {
                    affixData.Inherit(parent);
                }
            }

            roll = Math.Max(roll, 1);
            var affix = new Affix(character, affixData, tier, roll);

            if(affixData.Conditions != null)
            {
                foreach(var condName in affixData.Conditions)
                {
                    var condData = GameData.Conditions.First(x => x.Name == condName);
                    if(condData != null)
                    {
                        var condType = (ConditionType)Enum.Parse(typeof(ConditionType), condData.Type);
                        var condition = condData.Condition;
                        affix.AddNode(new ScriptedCondition(affix, condType, LuaContext, condition));
                        break;
                    }
                }
            }

            var affixType = (AffixType)Enum.Parse(typeof(AffixType), affixData.Type);

            switch(affixType)
            {
                case AffixType.AttributeModifier:
                    affix.AddNode(new AttributeModifier(affix));
                    break;
                case AffixType.Logic:
                    affix.AddNode(new LogicNode(affix, LuaContext, affixData));
                    break;
            }

            return affix;
        }
    }
}

// todo :put these in a better place

public class ConditionEntry
{
    public string Name;
    public string Type;
    public string Condition;
}

public class AffixDataEntry
{
    public int Tier;
    public string Name;
    public int Minimum;
    public int Maximum;
    public int Amount;
    public float Duration;
}

public class AffixEntry
{
    public string Name;
    public string Inherits;
    public string Type;
    public string Attribute;
    public string Slot;
    public string Description;
    public string Invoke;
    public AffixDataEntry[] Data;
    public string[] Conditions;
    public string[] Activators;
    public bool ForTalent;

    public void Inherit(AffixEntry parent)
    {
        Type = Type ?? parent.Type;
        Attribute = Attribute ?? parent.Attribute;
        Slot = Slot ?? parent.Slot;
        Description = Description ?? parent.Description;
        Invoke = Invoke ?? parent.Invoke;
        Data = Data ?? parent.Data;
        Conditions = Conditions ?? parent.Conditions;
        Activators = Activators ?? parent.Activators;
    }

    public string ParseDescription(int tier = 0, int roll = 0)
    {
        if(Description == null)
        {
            return "!NO DESCRIPTION!";
        }

        var desc = Description;

        if (desc.IndexOf("#roll") != -1)
        {
            var min = Data[tier].Minimum;
            var max = Data[tier].Maximum;
            var rollResult = AttributeModifier.Roll(min, max, roll);
            desc = desc.Replace("#roll", rollResult.ToString());
        }

        if (desc.IndexOf("#minimum") != -1)
        {
            desc = desc.Replace("#minimum", Data[tier].Minimum.ToString());
        }

        if (desc.IndexOf("#maximum") != -1)
        {
            desc = desc.Replace("#maximum", Data[tier].Maximum.ToString());
        }

        if (desc.IndexOf("#duration") != -1)
        {
            desc = desc.Replace("#duration", Data[tier].Duration.ToString());
        }

        if (desc.IndexOf("#amount") != -1)
        {
            desc = desc.Replace("#amount", Data[tier].Amount.ToString());
        }

        return desc;
    }
}

public class ImplicitAffixEntry
{
    public string Name;
    public int Tier;
}

public class ItemBaseEntry
{
    public string Name;
    public string Type;
    public string Slot;
    public ImplicitAffixEntry[] ImplicitAffixes;
    public int InventoryWidth;
    public int InventoryHeight;
    public string Icon;
    public string Model;
}

public class AttributeEntry
{
    public string Name;
    public string Category;
}

public class GameDataObject
{
    public string[] Classes;
    public AttributeEntry[] Attributes;
    public ConditionEntry[] Conditions;
    public AffixEntry[] Affixes;
    public TwigGraph[] SkillTrees;
    public string[] ItemSlots;
    public ItemBaseEntry[] ItemBases;
}

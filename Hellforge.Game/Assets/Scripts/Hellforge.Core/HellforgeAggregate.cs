using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using NLua;
using Newtonsoft.Json;
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

        private string _dataDirectory;

        public void ReloadData()
        {
            if(_dataDirectory == null)
            {
                throw new Exception("No data has been loaded yet");
            }
            LoadData(_dataDirectory);
        }

        public void LoadData(string dataDirectory, bool defaultToCompiled = true)
        {
            _dataDirectory = dataDirectory;

            LuaContext?.Dispose();
            LuaContext = new Lua();
            var dataSource = new JObject();

            var files = Directory.GetFiles(dataDirectory, "*.*", SearchOption.AllDirectories);
            var jsonFinished = false;

            if (defaultToCompiled)
            {
                var compiledFile = files.FirstOrDefault(x => x.EndsWith("compiled.json"));

                if (compiledFile != null)
                {
                    var source = File.ReadAllText(compiledFile);
                    dataSource.Merge(JObject.Parse(source));
                    jsonFinished = true;
                }
            }

            foreach (string file in files)
            {
                var ext = Path.GetExtension(file);
                if(ext == ".lua")
                {
                    LuaContext.DoFile(file);
                }
                else if(!jsonFinished && ext == ".json")
                {
                    var text = File.ReadAllText(file);
                    dataSource.Merge(JObject.Parse(text));
                }
            }

            GameData = dataSource.ToObject<GameDataObject>();

            foreach(var skillTree in GameData.TalentTrees)
            {
                skillTree.Hellforge = this;
                foreach(var node in skillTree.Nodes)
                {
                    node.Graph = skillTree;
                }
            }
        }

        public void CompileToSingleFile()
        {
            var filePath = _dataDirectory + "\\compiled.json";
            File.WriteAllText(filePath, JsonConvert.SerializeObject(GameData));
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
            var affixData = GameData.Affixes.FirstOrDefault(x => x.Name == name);

            if(affixData == null)
            {
                return null;
            }

            if(!string.IsNullOrEmpty(affixData.Inherits))
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
                        affix.AddNode(new ScriptedCondition(affix, condType, () => LuaContext, condition));
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
                    affix.AddNode(new LogicNode(affix, () => LuaContext, affixData));
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

public class SkillEntry
{
    public string Name;
    public string Archetype;
    public int MaxRank;
    public string Class;
    public string Group;
    public string Description;
    public string NextRank;
    public bool Passive;
    public JArray Data;

    public string ParseDescription(int rank)
    {
        if(rank > Data.Count - 1)
        {
            return Description;
        }
        return Parse(Description, Data[rank]);
    }

    public string ParseNextRankText(int rank)
    {
        if(NextRank == null)
        {
            return string.Empty;
        }
        if (rank > Data.Count - 1)
        {
            return string.Empty;
        }
        return Parse(NextRank, Data[rank]);
    }

    private string Parse(string input, JToken token)
    {
        var obj = (JObject)token;
        foreach(var parsedProperty in obj.Properties())
        {
            var name = parsedProperty.Name;
            var value = (string)parsedProperty.Value;
            input = input.Replace('#' + name + ';', "<color=yellow>" + value + "</color>");
        }
        return input;
    }
}

public class AffixEntry
{
    public string Name = "New Affix";
    public string Inherits;
    public string Type = "AttributeModifier";
    public string Attribute = "Attack";
    public string Slot = "Prefix";
    public string ItemSlot = "Ring";
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

public class MonsterEntry
{
    public string Name;
    public int Health;
    public Dictionary<string, int> Attributes;
}

public class GameDataObject
{
    public string[] Classes;
    public AttributeEntry[] Attributes;
    public ConditionEntry[] Conditions;
    public AffixEntry[] Affixes;
    public TwigGraph[] TalentTrees;
    public string[] ItemSlots;
    public ItemBaseEntry[] ItemBases;
    public SkillEntry[] Skills;
    public MonsterEntry[] Monsters;
}

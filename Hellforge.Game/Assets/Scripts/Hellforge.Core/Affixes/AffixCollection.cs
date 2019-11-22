//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace Hellforge.Core.Affixes
//{
//    public class AffixCollection 
//    {

//        private HellforgeAggregate _hellforge;

//        public AffixCollection(HellforgeAggregate hellforge)
//        {
//            _hellforge = hellforge;
//        }

//        public Affix GenerateAffix(string name, int tier = 0, int roll = 0)
//        {
//            var affixData = _hellforge.GameData.Affixes.First(x => x.Name == name);

//            if (affixData == null)
//            {
//                return null;
//            }

//            if (affixData.Inherits != null)
//            {
//                var parent = _hellforge.GameData.Affixes.First(x => x.Name == affixData.Inherits);
//                if (parent != null)
//                {
//                    affixData.Inherit(parent);
//                }
//            }

//            roll = Math.Max(roll, 1);
//            var affix = new Affix(character, affixData, tier, roll);

//            if (affixData.Conditions != null)
//            {
//                foreach (var condName in affixData.Conditions)
//                {
//                    var condData = _hellforge.GameData.Conditions.First(x => x.Name == condName);
//                    if (condData != null)
//                    {
//                        var condType = (ConditionType)Enum.Parse(typeof(ConditionType), condData.Type);
//                        var condition = condData.Condition;
//                        affix.AddNode(new ScriptedCondition(affix, condType, _hellforge.LuaContext, condition));
//                        break;
//                    }
//                }
//            }

//            var affixType = (AffixType)Enum.Parse(typeof(AffixType), affixData.Type);

//            switch (affixType)
//            {
//                case AffixType.AttributeModifier:
//                    affix.AddNode(new AttributeModifier(affix));
//                    break;
//                case AffixType.Logic:
//                    affix.AddNode(new LogicNode(affix, _hellforge.LuaContext, affixData));
//                    break;
//            }

//            return affix;
//        }

//    }
//}


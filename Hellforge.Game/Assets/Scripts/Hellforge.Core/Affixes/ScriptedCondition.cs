using System;
using System.Linq;
using System.Collections.Generic;
using NLua;

namespace Hellforge.Core.Affixes
{
    public enum ConditionType
    {
        None,
        Persister,
        TargetFilter,
        DefenseFilter,
        TalentFilter
    }
    public class ScriptedCondition : AffixNode
    {

        public readonly ConditionType ConditionType;
        public readonly string Condition;
        public readonly string[] PostParams;

        public ScriptedCondition(Affix affix, ConditionType type, Func<Lua> getLuaContext, string condition, string[] postParams = null)
            : base(affix, getLuaContext)
        {
            ConditionType = type;
            Condition = condition;
            PostParams = postParams;
        }

        public override void Update()
        {
            // do nothing
        }

        public bool Passes()
        {
            var parenthesisIdx = Condition.IndexOf('(');
            var funcName = Condition.Substring(0, parenthesisIdx);
            var parameterList = Condition.Substring(parenthesisIdx + 1, Condition.IndexOf(')') - parenthesisIdx - 1);
            var parts = Array.ConvertAll(parameterList.Split(','), p => p.Trim());
            var callParams = new List<object>();

            foreach (var part in parts)
            {
                callParams.Add(Affix.Character.Entity.GetContext(part));
            }

            callParams.AddRange(GetPostParams());

            if (_getLuaContext()[funcName] is LuaFunction func)
            {
                var result = func.Call(callParams.ToArray());
                if (result != null)
                {
                    return (bool)result[0];
                }
            }

            return false;
        }

        public string GetDescription()
        {
            var parenthesisIdx = Condition.IndexOf('(');
            var condName = Condition;
            if(parenthesisIdx != -1)
            {
                condName = Condition.Substring(0, parenthesisIdx);
            }

            var condEntry = Affix.Character.Hellforge.GameData.Conditions.First(x => x.Name == condName);
            if (condEntry.Description == null)
            {
                return "!MISSING DESCRIPTION!";
            }

            var pp = GetPostParams();
            if (pp == null)
            {
                return condEntry.Description;
            }

            var result = condEntry.Description;

            for (int i = 0; i < pp.Count; i++)
            {
                result = result.Replace("$pp" + (i + 1), pp[i].ToString());
            }

            return result;
        }

        private List<object> GetPostParams()
        {
            if(PostParams == null)
            {
                return null;
            }

            var result = new List<object>();

            if (PostParams != null)
            {
                foreach (var pp in PostParams)
                {
                    if (pp[0] == '#')
                    {
                        var tierData = Affix.AffixData.Data[Affix.Tier];
                        var key = pp.Replace("#", null);
                        if (tierData.Variables != null
                            && tierData.Variables.ContainsKey(key))
                        {
                            result.Add(tierData.Variables[key]);
                        }
                    }
                    else
                    {
                        result.Add(pp);
                    }
                }
            }

            return result;
        }

    }
}

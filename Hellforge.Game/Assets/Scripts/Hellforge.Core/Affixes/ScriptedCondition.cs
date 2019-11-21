using System;
using System.Collections.Generic;
using NLua;
using Hellforge.Core.Entities;

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
        public readonly Lua LuaContext;
        public readonly string Condition;

        public ScriptedCondition(Affix affix, ConditionType type, Lua luaContext, string condition)
            : base(affix)
        {
            ConditionType = type;
            LuaContext = luaContext;
            Condition = condition;
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

            if (LuaContext[funcName] is LuaFunction func)
            {
                var result = func.Call(callParams.ToArray());
                if (result != null)
                {
                    return (bool)result[0];
                }
            }

            return false;
        }

    }
}

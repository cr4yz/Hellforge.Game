using System;
using System.Collections.Generic;
using NLua;

namespace Hellforge.Core.Affixes
{
    public class LogicNode : AffixNode
    {

        private readonly AffixEntry _data;
        private readonly Lua _lua;

        public LogicNode(Affix affix, Lua lua, AffixEntry affixData)
            : base(affix)
        {
            _data = affixData;
            _lua = lua;
        }

        public override void Activate()
        {
            var invoke = _data.Invoke;

            if(invoke != null)
            {
                var parenthesisIdx = invoke.IndexOf('(');
                var funcName = invoke.Substring(0, parenthesisIdx);
                var parameterList = invoke.Substring(parenthesisIdx + 1, invoke.IndexOf(')') - parenthesisIdx - 1);
                var parts = Array.ConvertAll(parameterList.Split(','), p => p.Trim());
                var callParams = new List<object>();

                foreach (var part in parts)
                {
                    if (part[0] == '$')
                    {
                        callParams.Add(Affix.Character.Entity.GetContext(part));
                    }
                    else if (part[0] == '#')
                    {
                        var field = part.Substring(1, part.Length - 1).ToLower();
                        switch(field)
                        {
                            case "minimum":
                                callParams.Add(_data.Data[Affix.TierDataIndex].Minimum);
                                break;
                            case "maximum":
                                callParams.Add(_data.Data[Affix.TierDataIndex].Maximum);
                                break;
                            case "roll":
                                var min = _data.Data[Affix.TierDataIndex].Minimum;
                                var max = _data.Data[Affix.TierDataIndex].Maximum;
                                var roll = Affix.Roll;
                                callParams.Add(AttributeModifier.Roll(min, max, roll));
                                break;
                        }
                    }
                }

                if (_lua[funcName] is LuaFunction func)
                {
                    func.Call(callParams.ToArray());
                }
            }
        }

    }
}

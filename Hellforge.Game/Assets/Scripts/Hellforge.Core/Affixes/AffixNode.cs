using System;
using NLua;

namespace Hellforge.Core.Affixes
{
    public class AffixNode
    {

        public readonly Affix Affix;
        protected Func<Lua> _getLuaContext;

        public AffixNode(Affix affix, Func<Lua> getLuaContext = null)
        {
            Affix = affix;
            _getLuaContext = getLuaContext;
        }

        public virtual void Update() { }
        public virtual void Activate() { }
        public virtual void Enable() { }
        public virtual void Disable() { }
    }
}

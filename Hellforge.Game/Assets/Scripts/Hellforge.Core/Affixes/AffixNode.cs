namespace Hellforge.Core.Affixes
{
    public class AffixNode
    {

        public readonly Affix Affix;

        public AffixNode(Affix affix)
        {
            Affix = affix;
        }

        public virtual void Update() { }
        public virtual void Activate() { }
        public virtual void Enable() { }
        public virtual void Disable() { }
    }
}

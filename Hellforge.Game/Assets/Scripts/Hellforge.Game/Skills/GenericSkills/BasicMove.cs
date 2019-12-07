using Hellforge.Core;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class BasicMove : BaseSkill
    {

        public BasicMove(D4Hero hero)
            : base(hero, "BasicMove")
        {
            castWithoutTarget = true;
            BlocksInput = false;
        }

    }
}


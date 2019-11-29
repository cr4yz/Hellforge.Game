using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Entities
{
    public class InteractableNPC : BaseEntity, IInteractable
    {

        protected override void _Start()
        {
            selectable = true;
            SetOutlineColor(1);
        }

        public void Interact()
        {
            Debug.Log("INTERACT!");
        }

    }
}


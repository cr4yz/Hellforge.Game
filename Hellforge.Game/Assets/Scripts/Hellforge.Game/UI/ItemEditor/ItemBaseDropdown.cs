using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class ItemBaseDropdown : MonoBehaviour
    {

        void Start()
        {
            if(D4Data.Instance == null)
            {
                return;
            }

            var dropdown = GetComponent<Dropdown>();
            var bases = new List<string>();
            foreach(var itemBase in D4Data.Instance.Hellforge.GameData.ItemBases)
            {
                bases.Add(itemBase.Name);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(bases);
        }

    }
}


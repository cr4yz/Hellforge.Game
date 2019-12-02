using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class ItemAffixDropdown : MonoBehaviour
    {

        void Awake()
        {
            if (D4Data.Instance == null)
            {
                Debug.LogError("Hellforge not loaded yet..");
                return;
            }

            var dropdown = GetComponent<Dropdown>();
            var affixes = new List<string>();
            foreach (var affix in D4Data.Instance.Hellforge.GameData.Affixes)
            {
                if(affix.ForTalent)
                {
                    continue;
                }
                affixes.Add(affix.Name);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(affixes);
        }

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class ItemSlotDropdown : MonoBehaviour
    {

        void Awake()
        {
            if (D4Data.Instance == null)
            {
                Debug.LogError("Hellforge not loaded yet..");
                return;
            }

            var dropdown = GetComponent<Dropdown>();
            var slots = new List<string>();
            foreach (var slot in D4Data.Instance.Hellforge.GameData.ItemSlots)
            {
                slots.Add(slot);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(slots);
        }

    }
}


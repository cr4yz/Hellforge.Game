using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class AttributeDropdown : MonoBehaviour
    {

        void Awake()
        {
            if (D4Data.Instance == null)
            {
                Debug.LogError("Hellforge not loaded yet..");
                return;
            }

            var dropdown = GetComponent<Dropdown>();
            var attributes = new List<string>();
            foreach (var attr in D4Data.Instance.Hellforge.GameData.Attributes)
            {
                attributes.Add(attr.Name);
            }
            dropdown.ClearOptions();
            dropdown.AddOptions(attributes);
        }

    }
}


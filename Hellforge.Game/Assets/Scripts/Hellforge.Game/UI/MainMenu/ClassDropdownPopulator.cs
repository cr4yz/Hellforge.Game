using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(Dropdown))]
    public class ClassDropdownPopulator : MonoBehaviour
    {
        void Start()
        {
            var dropdown = GetComponent<Dropdown>();
            dropdown.ClearOptions();
            dropdown.AddOptions(D4Data.Instance.Hellforge.GameData.Classes.ToList());
        }
    }
}


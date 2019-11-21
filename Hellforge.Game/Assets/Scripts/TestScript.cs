using System.Linq;
using UnityEngine;
using Hellforge.Core;
using Hellforge.Core.Entities;
using Hellforge.Core.Twig;
using Hellforge.Game.Twig;

public class TestScript : MonoBehaviour
{

    [SerializeField]
    private TwigGraphRenderer _twigGraph;
    private HellforgeAggregate _hellforge;

    // Start is called before the first frame update
    void Start()
    {
        _hellforge = new HellforgeAggregate();
        _hellforge.LoadData(Application.dataPath + "/HellforgeData/Diablo4");

        var barbTree = _hellforge.GameData.SkillTrees.First(x => x.Class == "Barbarian");

        if(barbTree != null)
        {
            InstantiateTalentTree(barbTree);
        }
    }

    void InstantiateTalentTree(TwigGraph graph)
    {
        var character = new Character();
        _twigGraph.Render(graph, character);
    }
}

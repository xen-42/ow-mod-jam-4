using System.Linq;
using UnityEngine;

namespace EscapeRoomJam4;

public class ChromeToSandstoneReplacer : MonoBehaviour
{
    private Material _copper, _sandstone, _sandstoneDark;

    public void Awake()
    {
        _copper = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_Copper_mat"));
        _sandstone = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_SandStone_mat"));
        _sandstoneDark = Resources.FindObjectsOfTypeAll<Material>().First(x => x.name.Contains("Structure_NOM_SandStone_Dark_mat"));

        foreach (var renderer in this.GetComponentsInChildren<Renderer>())
        {
            renderer.materials = renderer.materials.Select(GetReplacementMaterial).ToArray();
        }
    }

    private Material GetReplacementMaterial(Material material)
    {
        if (material.name.Contains("Structure_NOM_PorcelainClean_mat"))
        {
            return _sandstone;
        }
        if (material.name.Contains("Structure_NOM_Silver_mat"))
        {
            return _copper;
        }
        if (material.name.Contains("Structure_NOM_SilverPorcelain_mat"))
        {
            return _sandstoneDark;
        }
        return material;
    }
}

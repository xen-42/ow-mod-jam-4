using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoomJam4.GhostPuzzle
{
    [HarmonyPatch]
    public class DreamObjectVisibilityPatch
    {
        [HarmonyPrefix, HarmonyPatch(typeof(DreamObjectProjection), nameof(DreamObjectProjection.UpdateVisibility))]
        private static void DreamObjectProjection_UpdateVisibility_Prefix(DreamObjectProjection __instance)
        {
            ProjectionData data = __instance.GetComponent<ProjectionData>();
            if (data != null) data.OnVisibilityChanged();
        }
    }
}

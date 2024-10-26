using HarmonyLib;
using UnityEngine;

namespace EscapeRoomJam4
{

    [HarmonyPatch]
    public class ShipLogPatches
    {
        [HarmonyPrefix, HarmonyPatch(typeof(ShipLogController), nameof(ShipLogController.OnPressInteract))]
        public static void ShipLogController_OnPressInteract_Prefix(ShipLogController __instance)
        {
            Transform mapModeRoot = __instance.transform.Find("ShipLogPivot/ShipLogCanvas/MapMode/ScaleRoot/PanRoot");
            foreach (Transform planet in mapModeRoot.transform)
            {
                foreach (Transform child in planet)
                {
                    if (child.gameObject.name == "Line_ShipLog")
                    {
                        child.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}

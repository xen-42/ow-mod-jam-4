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
            if (!EscapeRoomJam4.InEscapeSystem())
            {
                return;
            }

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

        private static int _rowIndex, _objIndex;

        [HarmonyPrefix, HarmonyPatch(typeof(ShipLogMapMode), nameof(ShipLogMapMode.UpdateMapNavigation))]
        public static void ShipLogMapMode_UpdateMapNavigation(ShipLogMapMode __instance)
        {
            if (!EscapeRoomJam4.InEscapeSystem())
            {
                return;
            }

            // Track these because they get modified by the time ShipLogMapMode_GetClosestIndexInRow is called
            _rowIndex = __instance._rowIndex;
            _objIndex = __instance._objIndex;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ShipLogMapMode), nameof(ShipLogMapMode.GetClosestIndexInRow))]
        public static void ShipLogMapMode_GetClosestIndexInRow(ShipLogMapMode __instance, float xPos, int rowIndex, bool requireVisible, ref int __result)
        {
            if (!EscapeRoomJam4.InEscapeSystem())
            {
                return;
            }

            EscapeRoomJam4.Instance.ModHelper.Console.WriteLine($"Row {__instance._rowIndex}, col {__instance._objIndex}");
            // Only allow the player to directly move up from row 1 at column 8
            if (_rowIndex == 1 && _objIndex != 8)
            {
                __instance._rowIndex = 1;
                __result = __instance._objIndex;
            }
        }
    }
}




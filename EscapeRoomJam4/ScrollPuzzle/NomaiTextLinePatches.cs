using HarmonyLib;
using UnityEngine;

namespace EscapeRoomJam4.ScrollPuzzle;

[HarmonyPatch(typeof(NomaiTextLine))]
public static class NomaiTextLinePatches
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(NomaiTextLine.DetermineTextLineColor))]
    public static void NomaiTextLine_DetermineTextLineColor(NomaiTextLine __instance, NomaiTextLine.VisualState state, ref Color __result)
    {
        if (EscapeRoomJam4.InEscapeSystem())
        {
            var a = __result.a;
            if (__instance == ScrollPuzzleController.Red)
            {
                __result = Color.red;
            }
            else if (__instance == ScrollPuzzleController.Green)
            {
                __result = Color.green;
            }
            else if (__instance == ScrollPuzzleController.Blue)
            {
                __result = Color.blue;
            }
            __result.a = a;
        }
    }
}

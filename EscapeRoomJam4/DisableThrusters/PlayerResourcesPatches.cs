using HarmonyLib;

namespace EscapeRoomJam4;

[HarmonyPatch(typeof(PlayerResources))]
public static class PlayerResourcesPatches
{
    public static bool JetpackDisabled;

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerResources.IsJetpackUsable))]
    public static bool PlayerResources_IsJetpackUsable(PlayerResources __instance, ref bool __result)
    {
        if (EscapeRoomJam4.InEscapeSystem() && JetpackDisabled)
        {
            __result = false;
            return false;
        }
        else
        {
            return true;
        }
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(PlayerResources.IsBoosterAllowed))]
    public static bool PlayerResources_IsBoosterAllowed(PlayerResources __instance, ref bool __result)
    {
        if (EscapeRoomJam4.InEscapeSystem() && JetpackDisabled)
        {
            __result = false;
            return false;
        }
        else
        {
            return true;
        }
    }
}

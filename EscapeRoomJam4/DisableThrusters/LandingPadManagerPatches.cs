using HarmonyLib;

namespace EscapeRoomJam4.DisableThrusters;

[HarmonyPatch(typeof(LandingPadManager))]
public static class LandingPadManagerPatches
{
    public static bool ThrustersDisabled { get; set; }

    [HarmonyPrefix, HarmonyPatch(nameof(LandingPadManager.IsLanded))]
    public static bool LandingPadManager_IsLanded(LandingPadManager __instance, ref bool __result)
    {
        if (EscapeRoomJam4.InEscapeSystem() && ThrustersDisabled)
        {
            // If the ship is landed, then it plays the ignition sound effect
            // Convincing it that it isn't landed is the easiest way to avoid this sfx being triggered
            __result = false;
            return false;
        }
        else
        {
            return true;
        }
    }
}

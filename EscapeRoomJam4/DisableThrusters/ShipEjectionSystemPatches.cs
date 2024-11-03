using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4.DisableThrusters;

[HarmonyPatch(typeof(ShipEjectionSystem))]
public static class ShipEjectionSystemPatches
{
    [HarmonyPostfix, HarmonyPatch(nameof(ShipEjectionSystem.OnPressInteract))]
    public static void ShipEjectionSystem_OnPressInteract(ShipEjectionSystem __instance)
    {
        if (EscapeRoomJam4.InEscapeSystem())
        {
            if (!__instance._cockpitModule.isDetached && __instance._ejectPressed)
            {
                EscapeRoomJam4.Instance.StartCoroutine(GameOver());
            }
        }
    }

    private static IEnumerator GameOver()
    {
        yield return new WaitForSeconds(6f);

        DialogueConditionManager.s_instance.SetConditionState("WYRM_XEN_JAM_4_ALT_ENDING", true);
        GameObject.Find("EscapeShip_Body/Sector/KazooCreditsVolume").transform.localPosition = Vector3.zero;
    }
}

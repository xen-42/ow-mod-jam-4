using HarmonyLib;
using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4.DisableThrusters;

[HarmonyPatch]
public class GameOverManager : MonoBehaviour
{
    private static GameOverManager _instance;
    
    private bool _isGameOver;

    public void Awake()
    {
        _instance = this;
        GlobalMessenger.AddListener("ShipSystemFailure", OnShipSystemFailure);
    }

    public void OnDestroy()
    {
        GlobalMessenger.RemoveListener("ShipSystemFailure", OnShipSystemFailure);
    }

    private void OnShipSystemFailure()
    {
        StartCoroutine(GameOver());
    }

    [HarmonyPostfix, HarmonyPatch(typeof(ShipEjectionSystem), nameof(ShipEjectionSystem.OnPressInteract))]
    public static void ShipEjectionSystem_OnPressInteract(ShipEjectionSystem __instance)
    {
        if (EscapeRoomJam4.InEscapeSystem())
        {
            if (!__instance._cockpitModule.isDetached && __instance._ejectPressed)
            {
                _instance.OnShipSystemFailure();
            }
        }
    }

    private IEnumerator GameOver()
    {
        if (_isGameOver)
        {
            yield break;
        }
        else
        {
            _isGameOver = true;

            yield return new WaitForSeconds(6f);

            DialogueConditionManager.s_instance.SetConditionState("WYRM_XEN_JAM_4_ALT_ENDING", true);
            GameObject.Find("EscapeShip_Body/Sector/KazooCreditsVolume").transform.localPosition = Vector3.zero;
        }
    }
}

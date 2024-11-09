using HarmonyLib;
using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace EscapeRoomJam4;

[HarmonyPatch]
internal class ResurrectionController : MonoBehaviour
{
    [HarmonyPrefix, HarmonyPatch(typeof(DeathManager), nameof(DeathManager.KillPlayer))]
    public static bool DeathManager_KillPlayer(DeathManager __instance, DeathType deathType)
    {
        if (EscapeRoomJam4.InEscapeSystem())
        {
            if (!_instance._dying)
            {
                var deathDuration = deathType == DeathType.Meditation ? 3f : 0.5f;
                Locator.GetPlayerDeathAudio().PlayDeathAudio(deathType, deathDuration);
                _instance.StartCoroutine(_instance.DeathCoroutine(deathDuration));
                _instance._dying = true;
            }

            return false;
        }
        return true;
    }

    private PlayerCameraEffectController _effectController;
    private SpawnPoint _spawnPoint;
    private static ResurrectionController _instance;
    private bool _dying;
    private PlayerResources _resources;
    private HatchController _hatch;

    public void Start()
    {
        _instance = this;
        _effectController = Locator.GetPlayerCamera().GetComponent<PlayerCameraEffectController>();
        _spawnPoint = transform.Find("PlayerSpawnPoint").GetComponentInChildren<SpawnPoint>();
        _resources = Locator.GetPlayerBody().GetComponent<PlayerResources>();
        _hatch = GameObject.FindObjectOfType<HatchController>();
    }

    public IEnumerator DeathCoroutine(float eyeCloseTime)
    {
        _effectController.CloseEyes(eyeCloseTime);
        OWInput.ChangeInputMode(InputMode.None);
        ReticleController.Hide();
        Locator.GetPromptManager().SetPromptsVisible(false);
        yield return new WaitForSeconds(eyeCloseTime);

        if (Locator.GetPlayerBody().transform.parent?.GetComponent<PlayerAttachPoint>() is PlayerAttachPoint attachPoint)
        {
            attachPoint.DetachPlayer();
        }
        if (PlayerState.AtFlightConsole())
        {
            Locator.GetPlayerCameraController()._shipController.ExitFlightConsole();
            _hatch._isPlayerInShip = false;
            GlobalMessenger.FireEvent("ExitShip");
            _hatch.OpenHatch();
        }
        // Reset the suit state else 
        if (PlayerState.IsWearingSuit())
        {
            Locator.GetPlayerSuit().RemoveSuit(true);
            Locator.GetPlayerSuit().SuitUp(false, true, true);
        }

        Locator.GetPlayerBody().GetComponent<PlayerBody>().WarpToPositionRotation(_spawnPoint.transform.position, _spawnPoint.transform.rotation);

        _resources._currentHealth = PlayerResources._maxHealth;
        _resources._currentFuel = PlayerResources._maxFuel;
        _resources._currentOxygen = PlayerResources._maxOxygen;

        _resources.StartRefillResources(true, true);

        yield return new WaitForSeconds(3f);

        Locator.GetAudioMixer()._deathMixed = false;
        Locator.GetAudioMixer()._endTimesVolume.FadeTo(1f, 3f);
        Locator.GetAudioMixer()._nonEndTimesVolume.FadeTo(1f, 3f);

        Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(eyeCloseTime >= 3f ? AudioType.PlayerGasp_Light : AudioType.PlayerGasp_Medium, 1f);

        _effectController.OpenEyes(eyeCloseTime);
        yield return new WaitForSeconds(eyeCloseTime);
        OWInput.ChangeInputMode(InputMode.Character);

        _resources.StopRefillResources();

        ReticleController.Show();
        Locator.GetPromptManager().SetPromptsVisible(true);

        _instance._dying = false;
    }
}

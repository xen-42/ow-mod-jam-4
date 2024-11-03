using EscapeRoomJam4.DisableThrusters;
using UnityEngine;

namespace EscapeRoomJam4;

public class PropulsionDisabledController : MonoBehaviour
{
    public static PropulsionDisabledController instance;

    private NotificationData _shipAndJetpackDisabled;
    private ShipThrusterComponent[] _shipThrusters;

    public void Awake()
    {
        _shipAndJetpackDisabled = new(NotificationTarget.Ship, EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PROPULSION_DISABLED_NOTIF"), 0f, true);
    }

    public void Start()
    {
        instance = this;
        _shipThrusters = GameObject.FindObjectsOfType<ShipThrusterComponent>();
    }

    public void OnDestroy()
    {
        TurnOff();
    }

    public void TurnOff()
    {
        //PlayerResourcesPatches.JetpackDisabled = false;
        LandingPadManagerPatches.ThrustersDisabled = false;
        ProbeLauncherPatches.ProbeLauncherDisabled = false;
        NotificationManager.SharedInstance.UnpinNotification(_shipAndJetpackDisabled);
        foreach (var thruster in _shipThrusters)
        {
            thruster._thrusterModel?.SetThrusterBankEnabled(thruster._thrusterBank, true);
        }
    }

    public void TurnOn()
    {
        //PlayerResourcesPatches.JetpackDisabled = true;
        LandingPadManagerPatches.ThrustersDisabled = true;
        ProbeLauncherPatches.ProbeLauncherDisabled = true;
        NotificationManager.SharedInstance.PostNotification(_shipAndJetpackDisabled, true);
        foreach (var thruster in _shipThrusters)
        {
            // Ensure that they aren't damaged else they could be repaired to be re-enabled
            thruster.SetDamaged(false);

            thruster._thrusterModel?.SetThrusterBankEnabled(thruster._thrusterBank, false);
        }
    }
}

using UnityEngine;

namespace EscapeRoomJam4;

public class PropulsionDisabledController : MonoBehaviour
{
    private NotificationData _shipAndJetpackDisabled;
    private ShipThrusterComponent[] _shipThrusters;

    public void Awake()
    {
        _shipAndJetpackDisabled = new(NotificationTarget.Ship, EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PROPULSION_DISABLED_NOTIF"), 0f, true);
        _shipThrusters = GameObject.FindObjectsOfType<ShipThrusterComponent>();
    }

    public void OnDestroy()
    {
        PlayerResourcesPatches.JetpackDisabled = false;
    }

    public void TurnOn()
    {
        PlayerResourcesPatches.JetpackDisabled = false;
        NotificationManager.SharedInstance.UnpinNotification(_shipAndJetpackDisabled);
        foreach (var thruster in _shipThrusters)
        {
            thruster._thrusterModel?.SetThrusterBankEnabled(thruster._thrusterBank, true);
        }
    }

    public void TurnOff()
    {
        PlayerResourcesPatches.JetpackDisabled = true;
        NotificationManager.SharedInstance.PostNotification(_shipAndJetpackDisabled, true);
        foreach (var thruster in _shipThrusters)
        {
            // Ensure that they aren't damaged else they could be repaired to be re-enabled
            thruster.SetDamaged(false);

            thruster._thrusterModel?.SetThrusterBankEnabled(thruster._thrusterBank, false);
        }
    }
}

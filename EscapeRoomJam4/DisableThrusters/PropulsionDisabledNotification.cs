using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeRoomJam4.Jetpack;

public class PropulsionDisabledNotification : MonoBehaviour
{
    private NotificationData _notificationData;
    private float _cooldownTimer;

    public void Awake()
    {
        _notificationData = new(NotificationTarget.Player, EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PROPULSION_DISABLED_NOTIF"), 5f, true);
    }

    public void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.fixedUnscaledDeltaTime;
        }

        if (_cooldownTimer <= 0 
            && Locator.GetPlayerController()._isWearingSuit 
            && PlayerResourcesPatches.JetpackDisabled 
            && OWInput.IsInputMode(InputMode.Character)
            && OWInput.IsNewlyPressed(InputLibrary.thrustUp, InputMode.All))
        {
            Locator._playerAudioController.PlaySuitWarning();
            NotificationManager.SharedInstance.PostNotification(_notificationData);
            _cooldownTimer = 5;
        }
    }
}

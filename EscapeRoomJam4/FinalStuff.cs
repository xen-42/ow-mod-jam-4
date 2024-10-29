using EscapeRoomJam4.DisableThrusters;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class FinalStuff : MonoBehaviour
    {
        public GameObject[] stuffToDisable;
        // most stuff is handled via the animation, but this changes a few code-related things
        public void DoEnding()
        {
            DreamworldSkyController.instance.TurnOff();
            PropulsionDisabledController.instance.TurnOff();
            NotificationManager.SharedInstance.PostNotification(new NotificationData(EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PROPULSION_ENABLED")));

            foreach (GameObject go in stuffToDisable) go.SetActive(false);
        }
    }
}

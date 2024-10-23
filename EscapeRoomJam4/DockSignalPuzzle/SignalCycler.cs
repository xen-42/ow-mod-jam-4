using EscapeRoomJam4.DockSignalPuzzle;
using NewHorizons.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class SignalCycler : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer buttonRenderer;
        [SerializeField]
        private List<GameObject> signalObjects;
        [SerializeField, Range(0, 3)]
        private int id;

        private int activeSignal;
        private SingleInteractionVolume interactionVolume;

        private void Start()
        {
            for (int i = 0; i < signalObjects.Count; i++)
            {
                if (signalObjects[i].activeSelf)
                {
                    activeSignal = i;
                    // Activate once the jammer signal is identified
                    signalObjects[i].SetActive(false);
                    break;
                }
            }

            EscapeRoomJam4.WriteDebug($"Button {id} has an active signal {activeSignal}");

            interactionVolume = GetComponent<SingleInteractionVolume>();
            interactionVolume.ChangePrompt(EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PUSH_BUTTON"));
            interactionVolume.OnPressInteract += OnPressInteract;

            SignalController.instance.ChangeCorrectFlag(id, activeSignal == 0);

            if (PlayerData.KnowsSignal(SignalController.instance.JammerSignal))
            {
                EscapeRoomJam4.WriteDebug("Already identified jammer?");
                OnJammerIdentified();
            }
            else
            {
                SignalController.instance.UnlockAllSignals += OnJammerIdentified;
            }
        }

        private void OnJammerIdentified()
        {
            EscapeRoomJam4.WriteDebug("Identified jammer!");
            signalObjects[activeSignal].SetActive(true);
        }

        private void OnPressInteract()
        {
            var previousAudioSignal = signalObjects[activeSignal].GetComponentInChildren<AudioSignal>();
            signalObjects[activeSignal].SetActive(false);

            activeSignal++;
            if (activeSignal >= signalObjects.Count) activeSignal = 0;

            signalObjects[activeSignal].SetActive(true);

            var activeAudioSignal = signalObjects[activeSignal].GetComponentInChildren<AudioSignal>();
            if (!PlayerData.KnowsSignal(activeAudioSignal.GetName()))
            {
                PlayerData.LearnSignal(activeAudioSignal.GetName());
            }

            // force the Signalscope away since the signal seems to not play properly
            // TODO fix later?
            SignalController.instance.ResetSignalscope();

            // the first signal, 0, is always the correct one
            SignalController.instance.ChangeCorrectFlag(id, activeSignal == 0);

            var oldName = AudioSignal.SignalNameToString(previousAudioSignal._name);
            var newName = AudioSignal.SignalNameToString(activeAudioSignal._name);
            var formattedString = string.Format(EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("SIGNAL_SWAPPED"), oldName, newName).ToUpper();
            NotificationData data = new NotificationData(NotificationTarget.Player, formattedString, 3f, false);
            NotificationManager.SharedInstance.PostNotification(data, false);
            Locator.GetPlayerAudioController().PlayEnterLaunchCodes();
        }

        public void FinishPuzzle()
        {
            Material[] mats = buttonRenderer.materials;
            // Second material in the list is the button material
            mats[1].color = Color.green;
            mats[1].SetColor("_EmissionColor", Color.green);
            buttonRenderer.materials = mats;
            interactionVolume.DisableInteraction();
        }
    }
}

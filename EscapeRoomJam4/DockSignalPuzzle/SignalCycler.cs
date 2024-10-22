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
                    break;
                }
            }

            EscapeRoomJam4.WriteDebug($"Button {id} has an active signal {activeSignal}");

            interactionVolume = GetComponent<SingleInteractionVolume>();
            interactionVolume.ChangePrompt(EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PUSH_BUTTON"));
            interactionVolume.OnPressInteract += OnPressInteract;

            SignalController.instance.ChangeCorrectFlag(id, activeSignal == 0);
        }

        private void OnPressInteract()
        {
            signalObjects[activeSignal].SetActive(false);

            activeSignal++;
            if (activeSignal >= signalObjects.Count) activeSignal = 0;

            signalObjects[activeSignal].SetActive(true);

            // force the Signalscope away since the signal seems to not play properly
            // TODO fix later?
            Locator.GetToolModeSwapper().UnequipTool();

            // the first signal, 0, is always the correct one
            SignalController.instance.ChangeCorrectFlag(id, activeSignal == 0);
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

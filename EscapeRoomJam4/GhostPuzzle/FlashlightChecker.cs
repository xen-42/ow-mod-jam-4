using UnityEngine;

namespace EscapeRoomJam4.GhostPuzzle
{
    public class FlashlightChecker : MonoBehaviour
    {
        [SerializeField]
        private GameObject onFlashlightPrompt;
        [SerializeField]
        private GameObject offFlashlightParent;
        [SerializeField]
        private LookAtPrompt offFlashlightPrompt;

        private DreamObjectProjector projector;

        private void Start()
        {
            projector = GetComponentInParent<DreamObjectProjector>();
            projector.OnProjectorExtinguished += ExtinguishPrompt;
            projector.OnProjectorLit += LightPrompt;
            onFlashlightPrompt.SetActive(true);
            offFlashlightParent.SetActive(false);
            offFlashlightPrompt.SetPromptActive(false);
            GlobalMessenger.AddListener("TurnOnFlashlight", TurnOnFlashlight);
            GlobalMessenger.AddListener("TurnOffFlashlight", TurnOffFlashlight);
        }

        private void Destroy()
        {
            GlobalMessenger.RemoveListener("TurnOnFlashlight", TurnOnFlashlight);
            GlobalMessenger.RemoveListener("TurnOffFlashlight", TurnOffFlashlight);
        }

        public void ExtinguishPrompt()
        {
            onFlashlightPrompt.SetActive(true);
            offFlashlightParent.SetActive(false);
        }

        public void LightPrompt()
        {
            onFlashlightPrompt.SetActive(false);
            offFlashlightParent.SetActive(true);
        }

        public void TurnOffFlashlight()
        {
            offFlashlightPrompt.SetPromptActive(false);
        }

        public void TurnOnFlashlight()
        {
            offFlashlightPrompt.SetPromptActive(true);
        }
    }
}

using UnityEngine;

namespace EscapeRoomJam4
{
    public class LookAtPrompt : MonoBehaviour
    {
        [Tooltip("String ID that will be translated and show. Uses UI Dictionary.")]
        public string promptText;

        private SingleInteractionVolume interactionVolume;

        private void Start()
        {
            string translatedPrompt = EscapeRoomJam4.Instance.NewHorizons.GetTranslationForUI(promptText);

            interactionVolume = gameObject.GetAddComponent<SingleInteractionVolume>();
            interactionVolume.SetKeyCommandVisible(false);
            interactionVolume.ChangePrompt(translatedPrompt);
            // prevent reaction to interact being pressed
            interactionVolume.OnGainFocus += () => interactionVolume.ChangePrompt(translatedPrompt);
        }

        private void OnDrawGizmosSelected()
        {
            if (GetComponent<InteractReceiver>() == null) return;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, GetComponent<InteractReceiver>()._interactRange);
        }

        public void SetPromptActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}

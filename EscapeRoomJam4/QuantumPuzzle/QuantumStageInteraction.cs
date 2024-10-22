using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4.QuantumPuzzle
{
    public class QuantumStageInteraction : MonoBehaviour
    {
        private SingleInteractionVolume interactReceiver;
        [SerializeField]
        private Renderer renderer;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private NomaiChest chest;

        private void Start()
        {
            interactReceiver = GetComponent<SingleInteractionVolume>();

            interactReceiver.OnPressInteract += OnPressInteract;

            interactReceiver.ChangePrompt("Press");
        }

        private void OnPressInteract()
        {
            if (QuantumPuzzleController.instance.IsSolved())
            {
                QuantumPuzzleController.instance.CheckIfSolved();
                animator.SetTrigger("PressCorrect");
                StartCoroutine(ChangeColor(Color.green, 1));
                interactReceiver.DisableInteraction();
                StartCoroutine(OpenChest());
            }
            else
            {
                animator.SetTrigger("PressIncorrect");
                StartCoroutine(ChangeColor(Color.red, 1));
                StartCoroutine(ChangeColor(Color.blue, 2));
                StartCoroutine(ChangeInteration());
            }
        }

        private IEnumerator ChangeColor(Color color, float delay)
        {
            yield return new WaitForSeconds(delay);
            renderer.material.color = color;
            renderer.material.SetVector("_EmissionColor", color);
        }

        private IEnumerator ChangeInteration()
        {
            interactReceiver.DisableInteraction();
            yield return new WaitForSeconds(2);
            interactReceiver.EnableInteraction();
        }

        private IEnumerator OpenChest()
        {
            yield return new WaitForSeconds(2);
            chest.Open();
        }
    }
}

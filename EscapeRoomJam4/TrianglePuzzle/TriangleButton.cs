using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeRoomJam4.TrianglePuzzle
{
    public class TriangleButton : MonoBehaviour
    {
        [HideInInspector]
        public bool isOn = false;

        [SerializeField]
        private bool isCorrectButton;

        private MeshRenderer renderer;
        private SingleInteractionVolume interactionVolume;
        private TriangleController controller;

        private void Start()
        {
            controller = TriangleController.instance;
            interactionVolume = GetComponent<SingleInteractionVolume>();
            renderer = GetComponent<MeshRenderer>();

            interactionVolume.ChangePrompt(EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("PUSH_BUTTON"));
            interactionVolume.OnPressInteract += OnPressInteract;
        }

        private void OnPressInteract()
        {
            isOn = !isOn;
            renderer.material = controller.GetStateMaterial(isOn);

            controller.CheckTriangle(isCorrectButton, isOn);

            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.Menu_LeftRight);
        }

        private void OnDrawGizmos()
        {
            if (isCorrectButton)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(transform.position, 0.1f);
            }
        }
    }
}

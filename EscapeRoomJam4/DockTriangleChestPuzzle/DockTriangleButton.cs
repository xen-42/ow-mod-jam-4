using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class DockTriangleButton : MonoBehaviour
    {
        [SerializeField, Range(0, 2)]
        private int id;

        [SerializeField]
        private TriangleColor defaultColor;

        private TriangleColor currentColor;

        private SingleInteractionVolume interactionVolume;

        private void Start()
        {
            interactionVolume = GetComponent<SingleInteractionVolume>();
            currentColor = defaultColor;
            SetColor(defaultColor);
            interactionVolume.OnPressInteract += OnPressInteract;
        }

        private void OnPressInteract()
        {
            int index = (int)currentColor;
            index++;
            // remove black
            if (index == 6) index = 7;
            if (index > 7) index = 0;
            currentColor = (TriangleColor)index;
            SetColor(currentColor);
            DockTrianglePuzzleController.instance.CycleTriangle(id, currentColor);
            Locator.GetPlayerAudioController()._oneShotExternalSource.PlayOneShot(AudioType.Menu_LeftRight);

            ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_ARROWS_IDENTIFY");
        }

        private void SetColor(TriangleColor color)
        {
            Color newColor = Color.white;
            switch (color)
            {
                case TriangleColor.Black:
                    newColor = Color.black;
                    break;
                case TriangleColor.Red:
                    newColor = Color.red;
                    break;
                case TriangleColor.Yellow:
                    newColor = Color.yellow;
                    break;
                case TriangleColor.Green:
                    newColor = Color.green;
                    break;
                case TriangleColor.Cyan:
                    newColor = Color.cyan;
                    break;
                case TriangleColor.Blue:
                    newColor = Color.blue;
                    break;
                case TriangleColor.Magenta:
                    newColor = Color.magenta;
                    break;
                default:
                    newColor = Color.white;
                    break;
            }
            GetComponent<MeshRenderer>().material.color = newColor;

            string newPrompt =  EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText($"BUTTON_{color.ToString().ToUpper()}");
            interactionVolume.ChangePrompt(newPrompt);
        }
    }
}

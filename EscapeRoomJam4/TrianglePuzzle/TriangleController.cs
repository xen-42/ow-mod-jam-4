using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeRoomJam4.TrianglePuzzle
{
    public class TriangleController : Puzzle
    {
        public static TriangleController instance;

        [SerializeField]
        private MeshRenderer onMaterial;
        [SerializeField]
        private MeshRenderer offMaterial;

        private bool isSolved = false;
        private int onCorrectButtons = 0;
        private int onIncorrectButtons = 0;
        private List<TriangleButton> allButtons;

        private void Awake()
        {
            instance = this;
        }

        public void RegisterTriangle(TriangleButton button)
        {
            if (allButtons == null) allButtons = new List<TriangleButton>();
            allButtons.Add(button);
        }

        private void ResetTriangles()
        {
            foreach (TriangleButton button in allButtons)
            {
                button.isOn = false;
                button.GetComponent<MeshRenderer>().material = GetStateMaterial(false);
            }
            onCorrectButtons = 0;
            onIncorrectButtons = 0;
        }

        public void CheckTriangle(bool isCorrectTriangle, bool turningOn)
        {
            if (onCorrectButtons + onIncorrectButtons >= 4) ResetTriangles();
            if (isCorrectTriangle) onCorrectButtons += turningOn ? 1 : -1;
            else onIncorrectButtons += turningOn ? 1 : -1;

            EscapeRoomJam4.WriteDebug($"Correct: {onCorrectButtons}, Incorrect: {onIncorrectButtons}");

            CheckIfSolved();
        }

        public override bool IsSolved()
        {
            if (isSolved) return false;
            if (onCorrectButtons == 4 && onIncorrectButtons == 0)
            {
                isSolved = true;
                ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_TRIANGLE_IDENTIFY");
                foreach (TriangleButton button in allButtons)
                {
                    button.GetComponent<SingleInteractionVolume>().DisableInteraction();
                }
                return true;
            }
            return false;
        }

        public Material GetStateMaterial(bool isOn)
        {
            if (isOn) return onMaterial.sharedMaterial;
            return offMaterial.sharedMaterial;
        }
    }
}

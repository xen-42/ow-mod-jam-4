using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomJam4
{
    public class DockTrianglePuzzleController : Puzzle
    {

        public UnityEvent SecretSolved = new();
        [HideInInspector]
        public static DockTrianglePuzzleController instance;

        [SerializeField]
        private TriangleColor[] mainSolution;
        [SerializeField]
        private TriangleColor[] secretSolution;
        [SerializeField]
        private SingleInteractionVolume[] buttons;

        private TriangleColor[] storedSolution = new TriangleColor[3];
        private bool normalSolved = false;
        private bool secretSolved = false;


        private void Awake()
        {
            instance = this;
        }

        public void CycleTriangle(int id, TriangleColor color)
        {
            storedSolution[id] = color;
            CheckIfSolved();
            if (normalSolved && secretSolved) DisableButtons();
        }

        private void DisableButtons()
        {
            foreach (SingleInteractionVolume button in buttons)
            {
                button.DisableInteraction();
            }
        }

        public override bool IsSolved()
        {
            if (IsSecretSolved())
            {
                SecretSolved.Invoke();
            }
            for (int i = 0; i < 3; i++)
            {
                if (mainSolution[i] != storedSolution[i]) return false;
            }
            ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_ARROWS_SOLVED");
            normalSolved = true;
            return true;
        }

        private bool IsSecretSolved()
        {
            for (int i = 0; i < 3; i++)
            {
                if (secretSolution[i] != storedSolution[i]) return false;
            }
            ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_ARROWS_SECRET");
            secretSolved = true;
            return true;
        }

        private void Reset()
        {
            mainSolution = new TriangleColor[3];
            mainSolution[0] = TriangleColor.Red;
            mainSolution[1] = TriangleColor.Red;
            mainSolution[2] = TriangleColor.Red;
            secretSolution = new TriangleColor[3];
            secretSolution[0] = TriangleColor.Red;
            secretSolution[1] = TriangleColor.Red;
            secretSolution[2] = TriangleColor.Red;
        }
    }
}

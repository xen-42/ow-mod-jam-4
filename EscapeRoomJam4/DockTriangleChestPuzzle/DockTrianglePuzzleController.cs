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

        private TriangleColor[] storedSolution = new TriangleColor[3];

        private void Awake()
        {
            instance = this;
        }

        public void CycleTriangle(int id, TriangleColor color)
        {
            storedSolution[id] = color;
            CheckIfSolved();
        }

        public override bool IsSolved()
        {
            EscapeRoomJam4.WriteDebug("Testing for solution...");
            if (IsSecretSolved())
            {
                SecretSolved.Invoke();
                EscapeRoomJam4.WriteDebug("Secret solved!");
            }
            for (int i = 0; i < 3; i++)
            {
                if (mainSolution[i] != storedSolution[i]) return false;
            }
            EscapeRoomJam4.WriteDebug("Main puzzle solved!");
            return true;
        }

        private bool IsSecretSolved()
        {
            for (int i = 0; i < 3; i++)
            {
                if (secretSolution[i] != storedSolution[i]) return false;
            }
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

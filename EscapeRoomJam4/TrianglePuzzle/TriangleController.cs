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

        private void Awake()
        {
            instance = this;
        }

        public void CheckTriangle(bool isCorrectTriangle, bool turningOn)
        {
            if (isCorrectTriangle) onCorrectButtons += turningOn ? 1 : -1;
            else onIncorrectButtons += turningOn ? 1 : -1;

            CheckIfSolved();
        }

        public override bool IsSolved()
        {
            if (isSolved) return false;
            if (onCorrectButtons == 4 && onIncorrectButtons == 0)
            {
                isSolved = true;
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

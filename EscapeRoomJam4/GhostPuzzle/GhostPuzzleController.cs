using EscapeRoomJam4.GhostPuzzle;
using System;
using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class GhostPuzzleController : Puzzle
    {
        private bool[] floorsEnabled = new bool[5];
        private bool isInitialized = false;
        private bool isSolved = false;

        [SerializeField]
        private DreamObjectProjection[] floors;
        [SerializeField]
        private GameObject inhabitantEndPoint;

        public static GhostPuzzleController instance;

        private void Start()
        {
            instance = this;
            StartCoroutine(Initialize());
        }

        public void ToggleFloors(int[] floorIDs)
        {
            if (isSolved) return;
            foreach (int id in floorIDs)
            {
                floorsEnabled[id] = !floorsEnabled[id];
                floors[id].SetVisible(floorsEnabled[id]);
            }
            if (isInitialized) CheckIfSolved();
        }

        public override bool IsSolved()
        {
            foreach (bool isEnabled in floorsEnabled)
            {
                if (!isEnabled) return false;
            }
            EscapeRoomJam4.WriteDebug("Ghost Puzzle solved!");
            isSolved = true;
            GhostWalkController.instance.WalkTo(inhabitantEndPoint.transform.position);
            return true;
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            int[] all = new int[] { 0, 1, 2, 3, 4 };
            ToggleFloors(all);
            ToggleFloors(all);
            isInitialized = true;
        }
    }
}

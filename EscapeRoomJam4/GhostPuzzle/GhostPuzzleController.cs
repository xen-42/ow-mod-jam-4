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
        private GameObject inhabitantStartPoint;
        [SerializeField]
        private GameObject inhabitantEndPoint;
        [SerializeField]
        private GameObject bridgeRoot;
        [SerializeField]
        private GameObject scream;

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
            if (isInitialized) CheckSolution();
        }

        public void CheckSolution()
        {
            foreach (bool isEnabled in floorsEnabled)
            {
                if (!isEnabled) return;
            }
            EscapeRoomJam4.WriteDebug("Ghost Puzzle solved!");
            isSolved = true;
            GhostWalkController.instance.WalkTo(inhabitantEndPoint.transform.position - inhabitantStartPoint.transform.position);
            StartCoroutine(SolvePuzzle());
        }

        public override bool IsSolved()
        {
            scream.SetActive(true);
            bridgeRoot.SetActive(false);
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

        private IEnumerator SolvePuzzle()
        {
            yield return new WaitForSeconds(15);
            CheckIfSolved();
        }
    }
}

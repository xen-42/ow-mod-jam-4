using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class ProjectionData : MonoBehaviour
    {
        public DreamObjectProjection[] linkedProjections;

        private int[] linkedProjectionIDs;
        private bool isInitialized = false;

        private void Start()
        {
            linkedProjectionIDs = new int[linkedProjections.Length];
            for (int id = 0; id < linkedProjectionIDs.Length; id++)
            {
                // should get the number in "FloorN"
                if (int.TryParse(linkedProjections[id].gameObject.name.Replace("Floor", ""), out int value))
                {
                    linkedProjectionIDs[id] = value;
                }
                else
                {
                    EscapeRoomJam4.WriteDebug($"Could not parse {linkedProjections[id].gameObject.name} into a number!");
                }
            }
            StartCoroutine(Initialize());
        }

        public void OnVisibilityChanged()
        {
            if (isInitialized) GhostPuzzleController.instance.ToggleFloors(linkedProjectionIDs);
        }

        private IEnumerator Initialize()
        {
            yield return new WaitForEndOfFrame();
            isInitialized = true;
        }
    }
}

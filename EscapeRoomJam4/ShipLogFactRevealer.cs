using UnityEngine;

namespace EscapeRoomJam4
{
    public class ShipLogFactRevealer : MonoBehaviour
    {
        public static ShipLogFactRevealer instance;

        private void Start()
        {
            instance = this;
        }

        public void RevealFact(string factID)
        {
            GameObject.FindObjectOfType<ShipLogManager>().RevealFact(factID);
        }
    }
}

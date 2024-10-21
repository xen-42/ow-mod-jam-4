using UnityEngine;

namespace EscapeRoomJam4
{
    public class NomaiChest : MonoBehaviour
    {
        public void Open()
        {
            GetComponent<Animator>().SetTrigger("Open");
            // Enable key collider
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoomJam4
{
    public class LockedDoor : MonoBehaviour
    {
        private void Update()
        {
#if DEBUG
            if (Keyboard.current[Key.O].wasPressedThisFrame) SetOpenState(true);
#endif
        }

        public void SetOpenState(bool open)
        {
            Animator animator = GetComponent<Animator>();
            if (open) animator.SetTrigger("Open");
            else animator.SetTrigger("Closed");
        }
    }
}

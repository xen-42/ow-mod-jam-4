using UnityEngine;

namespace EscapeRoomJam4
{
    public class LockedDoor : MonoBehaviour
    {
        private void Start()
        {
            // for debugging only now, remove when we can actually want to be able to open them
            SetOpenState(true);
        }

        public void SetOpenState(bool open)
        {
            Animator animator = GetComponent<Animator>();
            if (open) animator.SetTrigger("Open");
            else animator.SetTrigger("Closed");
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeRoomJam4
{
    public class LockedDoor : MonoBehaviour
    {
        private OWAudioSource _audio;

        public void Awake()
        {
            _audio = gameObject.GetAddComponent<OWAudioSource>();
            _audio.spatialBlend = 1f;
        }

        private void Update()
        {
#if DEBUG
            if (Keyboard.current[Key.O].wasPressedThisFrame) SetOpenState(true);
#endif
        }

        public void SetOpenState(bool open)
        {
            Animator animator = GetComponent<Animator>();
            if (open)
            {
                animator.SetTrigger("Open");
                _audio.PlayOneShot(AudioType.NomaiDoorStart);
            }
            else
            {
                animator.SetTrigger("Close");
            }
        }
    }
}

using NewHorizons.Components.Props;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class Lock : MonoBehaviour
    {
        public LockType type;

        private void Start()
        {
            NHItemSocket socket = GetComponentInParent<NHItemSocket>();
            if (socket != null)
            {
                socket.OnSocketablePlaced += OnSocket;
                LockController.instance.RegisterLock(this, type);
            }
        }

        private void OnSocket(OWItem item)
        {
            LockController.instance.OpenLock(this, type);
        }
    }
}

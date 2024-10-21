using NewHorizons.Components.Props;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class NomaiChest : MonoBehaviour
    {
        private NHItem _key;

        public void Start()
        {
            _key = GetComponentInChildren<NHItem>();
            if (_key != null)
            {
                _key.EnableInteraction(false);
            }
        }

        public void Open()
        {
            GetComponent<Animator>().SetTrigger("Open");

            if (_key != null)
            {
                _key.EnableInteraction(true);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

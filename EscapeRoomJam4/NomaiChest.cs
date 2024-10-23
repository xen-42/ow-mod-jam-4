using NewHorizons.Components.Props;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class NomaiChest : MonoBehaviour
    {
        public GameObject keyLocation;

        private NHItem _key;

        [SerializeField]
        // these values are only relevant in the editor
        private Mesh editorGizmoKeyMesh;
        [SerializeField, Range(0, 1)]
        private float editorGizmoKeyScale;

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

        public void SecretOpen()
        {
            GetComponent<Animator>().SetTrigger("SecretOpen");
        }

        private void OnDrawGizmos()
        {
            if (keyLocation != null && editorGizmoKeyMesh != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireMesh(editorGizmoKeyMesh, keyLocation.transform.position, keyLocation.transform.rotation, Vector3.one * editorGizmoKeyScale);
            }
        }

        private void Reset()
        {
            editorGizmoKeyScale = 0.15f;
        }
    }
}

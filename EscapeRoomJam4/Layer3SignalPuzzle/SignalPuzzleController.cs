using UnityEngine;

namespace EscapeRoomJam4
{
    public class SignalPuzzleController : MonoBehaviour
    {
        [SerializeField]
        private Transform originPoint;
        [SerializeField]
        private Transform closeTotem;
        [SerializeField]
        private Transform middleTotem;
        [SerializeField]
        private Transform farTotem;

        private void Start()
        {
            AlignTotems(farTotem);
        }

        public void AlignTotems(Transform comparePosition)
        {
            Vector3 angle = (comparePosition.position - originPoint.position).normalized;
            closeTotem.position = MultipliedVector(angle, closeTotem.position);
            middleTotem.position = MultipliedVector(angle, middleTotem.position);
        }

        private Vector3 MultipliedVector(Vector3 angle, Vector3 comparePosition)
        {
            float distance = (comparePosition - originPoint.position).magnitude;
            return (angle * distance) + originPoint.position;
        }

        private void OnDrawGizmos()
        {
            if (originPoint != null && farTotem != null)
            {
                Vector3 angle = (farTotem.position - originPoint.position).normalized;
                Vector3 closePosition = MultipliedVector(angle, closeTotem.position);
                Vector3 midPosition = MultipliedVector(angle, middleTotem.position);
                Vector3 farPosition = farTotem.position;
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(originPoint.position, farTotem.position);
                Gizmos.DrawWireSphere(farPosition, 1);
                Gizmos.DrawWireSphere(farPosition, 20);
                Gizmos.DrawWireSphere(farPosition, 50);
                if (closeTotem != null)
                {
                    Gizmos.DrawWireSphere(closePosition, 1);
                    Gizmos.DrawWireSphere(closePosition, 20);
                    Gizmos.DrawWireSphere(closePosition, 50);
                    Gizmos.DrawLine(closePosition, new Vector3(closePosition.x, -20, closePosition.z));
                }
                if (middleTotem != null)
                {
                    Gizmos.DrawWireSphere(midPosition, 1);
                    Gizmos.DrawWireSphere(midPosition, 20);
                    Gizmos.DrawWireSphere(midPosition, 50);
                    Gizmos.DrawLine(midPosition, new Vector3(midPosition.x, -20, midPosition.z));
                }
                Gizmos.DrawLine(farPosition, new Vector3(farPosition.x, -20, farPosition.z));
            }
        }
    }
}

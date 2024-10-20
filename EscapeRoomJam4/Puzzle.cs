using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomJam4;

public abstract class Puzzle : MonoBehaviour
{
    [SerializeField]
    public UnityEvent Solved = new UnityEvent();

    public abstract bool IsSolved();

    public void CheckIfSolved()
    {
        if (IsSolved())
        {
            Solved?.Invoke();
        }
    }
}

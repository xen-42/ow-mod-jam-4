using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomJam4;

public abstract class Puzzle : MonoBehaviour
{
    [SerializeField]
    public UnityEvent Solved = new UnityEvent();

    /// <summary>
    /// When solved make sure to call CheckIfSolved so that the Solved event gets called
    /// </summary>
    /// <returns></returns>
    public abstract bool IsSolved();

    public void CheckIfSolved()
    {
        if (IsSolved())
        {
            Solved?.Invoke();
        }
    }
}

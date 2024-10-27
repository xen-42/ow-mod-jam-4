using UnityEngine;
using UnityEngine.Events;

namespace EscapeRoomJam4;

public abstract class Puzzle : MonoBehaviour
{
    [SerializeField]
    public UnityEvent Solved = new UnityEvent();

    protected bool _wasSolved;

    /// <summary>
    /// When solved make sure to call CheckIfSolved so that the Solved event gets called
    /// </summary>
    /// <returns></returns>
    public abstract bool IsSolved();

    public void CheckIfSolved()
    {
        if (IsSolved())
        {
            _wasSolved = true;
            Solved?.Invoke();
        }
    }
}

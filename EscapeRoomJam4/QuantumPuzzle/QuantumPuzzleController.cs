using EscapeRoomJam4.QuantumPuzzle;
using UnityEngine;

namespace EscapeRoomJam4;

public class QuantumPuzzleController : Puzzle
{
    public static QuantumPuzzleController instance;

    private static SocketedQuantumObject _quantumObject1, _quantumObject2, _quantumObject3;

    public void Start()
    {
        instance = this;

        _quantumObject1 = transform.Find("QuantumStatue1").GetComponent<SocketedQuantumObject>();
        _quantumObject2 = transform.Find("QuantumStatue2").GetComponent<SocketedQuantumObject>();
        _quantumObject3 = transform.Find("QuantumStatue3").GetComponent<SocketedQuantumObject>();

        Solved.AddListener(() =>
        {
            EscapeRoomJam4.WriteDebug($"{nameof(QuantumPuzzleController)} - puzzle complete!");
            _quantumObject1.SetIsQuantum(false);
            _quantumObject2.SetIsQuantum(false);
            _quantumObject3.SetIsQuantum(false);
        });
    }

    public override bool IsSolved()
    {
        EscapeRoomJam4.WriteDebug($"{nameof(QuantumPuzzleController)} - Testing states");

        bool solved = _quantumObject1._occupiedSocket.name == "Socket 0" &&
            _quantumObject2._occupiedSocket.name == "Socket 1" &&
            _quantumObject3._occupiedSocket.name == "Socket 2";
        if (solved) ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_QUANTUM_IDENTIFY");
        return solved;
            
    }
}

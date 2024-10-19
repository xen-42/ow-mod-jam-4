using HarmonyLib;
using System;
using System.Net.Sockets;
using UnityEngine;
using System.Linq;

namespace EscapeRoomJam4;

[HarmonyPatch]
public class ERQuantumPuzzleController : MonoBehaviour
{
    private SocketedQuantumObject _quantumObject1, _quantumObject2, _quantumObject3;
    private OWRigidbody _planetRigidBody;
    public Action Solved;

    // Next time the player is looking at all the objects, check if they are in the right states
    private bool _flagCheckWhenAllVisible;

    private static Action<QuantumObject> _onQuantumStateChanged;

    public void Start()
    {
        _planetRigidBody = this.GetAttachedOWRigidbody();

        _quantumObject1 = transform.Find("QuantumStatue1").GetComponent<SocketedQuantumObject>();
        _quantumObject2 = transform.Find("QuantumStatue2").GetComponent<SocketedQuantumObject>();
        _quantumObject3 = transform.Find("QuantumStatue3").GetComponent<SocketedQuantumObject>();

        // Recolour for test
        _quantumObject1.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        _quantumObject2.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        _quantumObject3.GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        // Can't have 4 objects in 4 slots
        // Instead we have a duplicate of the final object for each slot, which appears when that slot is "empty"
        for (int i = 0; i < _quantumObject1._socketList.Count; i++)
        {
            var emptySocketObject = transform.Find($"QuantumStatueEmptyState{i + 1}").gameObject;
            var socket = _quantumObject1._socketList[i];
            socket._emptySocketObject = emptySocketObject;
            emptySocketObject.SetActive(socket._quantumObject == null);
            emptySocketObject.transform.parent = socket.transform;
            emptySocketObject.transform.localPosition = Vector3.zero;
            emptySocketObject.transform.localRotation = Quaternion.identity;

            // Recolour for test
            emptySocketObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        _onQuantumStateChanged += OnQuantumObjectStateCollapse;

        EscapeRoomJam4.WriteDebug($"{nameof(ERQuantumPuzzleController)} - The correct answer is red, blue, green, yellow");
    }

    public void OnDestroy()
    {
        _onQuantumStateChanged -= OnQuantumObjectStateCollapse;
    }

    public void Update()
    {
        if (_flagCheckWhenAllVisible)
        {
            if (_quantumObject1.IsVisible() && _quantumObject2.IsVisible() && _quantumObject3.IsVisible()) 
            {
                _flagCheckWhenAllVisible = false;
                if (TestStates())
                {
                    // Only lock in a solution if the player is looking so it doesn't get solved behind their back
                    EscapeRoomJam4.WriteDebug($"{nameof(ERQuantumPuzzleController)} - puzzle complete!");
                    _quantumObject1.SetIsQuantum(false);
                    _quantumObject2.SetIsQuantum(false);
                    _quantumObject3.SetIsQuantum(false);
                    Solved?.Invoke();
                }
            }
        }
    }

    private bool TestStates()
    {
        EscapeRoomJam4.WriteDebug($"{nameof(ERQuantumPuzzleController)} - Testing states");

        return
            _quantumObject1._occupiedSocket.name == "Socket 0" &&
            _quantumObject2._occupiedSocket.name == "Socket 1" &&
            _quantumObject3._occupiedSocket.name == "Socket 2";
    }

    private void OnQuantumObjectStateCollapse(QuantumObject obj)
    {
        if (obj.GetAttachedOWRigidbody() == _planetRigidBody)
        {
            EscapeRoomJam4.WriteDebug($"{nameof(ERQuantumPuzzleController)} - {obj.name} moved");
            _flagCheckWhenAllVisible = true;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuantumSocket), nameof(QuantumSocket.SetQuantumObject))]
    private static void QuantumSocket_SetQuantumObject(QuantumSocket __instance, QuantumObject qObj)
    {
        // Bit hacky, but theres no event on QuantumSockets or QuantumObjects for when they change as far as I can tell
        _onQuantumStateChanged?.Invoke(qObj);
    }
}

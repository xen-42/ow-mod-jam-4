using Epic.OnlineServices.UI;
using UnityEngine;

namespace EscapeRoomJam4.CoordinateInterfacePuzzle;

public class CoordinateInterfacePuzzleController : Puzzle
{
    private NomaiCoordinateInterface _interface;

    public void Awake()
    {
        GameObject.Destroy(GetComponentInChildren<EyeCoordinatePromptTrigger>().gameObject);
        _interface = GetComponent<NomaiCoordinateInterface>();

        _interface._raisePillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => _interface.SetPillarRaised(true, true);
        _interface._lowerPillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => _interface.SetPillarRaised(false, false);
        _interface._raisePillarSlot.OnSlotActivated += (NomaiInterfaceSlot slot) => CheckIfSolved();

        _interface._coordinateX = new int[] { 4, 0, 3, 2 };
        _interface._coordinateY = new int[] { 5, 4, 3, 2 };
        _interface._coordinateZ = new int[] { 4, 2, 0, 5 };

        Solved.AddListener(() => EscapeRoomJam4.WriteDebug("Solved!"));
    }

    public void Start()
    {
        // DEBUG - Remove later after moving it in unity
        transform.parent.Find("EscapeShip/Sector_Nomai/Geometry/QuantumHints/ToyShip").transform.localPosition = new Vector3(-10, 4.5f, 10);
    }

    public override bool IsSolved() => _interface.CheckEyeCoordinates();
}

using HarmonyLib;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace EscapeRoomJam4.CoordinateInterfacePuzzle;

[HarmonyPatch]
public class CoordinateInterfacePuzzleController : Puzzle
{
    private static CoordinateInterfacePuzzleController _instance;

    private NomaiCoordinateInterface _interface;

    private int[] _coordinateX = new int[] { 4, 0, 3, 2 };
    private int[] _coordinateY = new int[] { 5, 4, 3, 2 };
    private int[] _coordinateZ = new int[] { 4, 2, 0, 5 };

    public Action SecretSolution;

    private bool _secretSolved;

    public void Awake()
    {
        _instance = this;

        GameObject.Destroy(GetComponentInChildren<EyeCoordinatePromptTrigger>().gameObject);
        _interface = GetComponent<NomaiCoordinateInterface>();

        _interface._raisePillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => _interface.SetPillarRaised(true, true);
        _interface._lowerPillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => _interface.SetPillarRaised(false, false);
        _interface._lowerPillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => CheckIfSolved();
        _interface._lowerPillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) => CheckAlternateSolution();

        Solved.AddListener(() => EscapeRoomJam4.WriteDebug("Solved!"));
    }

    public IEnumerator Start()
    {
        var chest = transform.parent.Find("EscapeShip/Sector_Nomai/Geometry/CoordinateInterfaceNomaiChest").GetComponent<NomaiChest>();
        Solved.AddListener(chest.Open);

        var secretChest = transform.parent.Find("EscapeShip/Sector_Nomai/Geometry/CoordinateInterfaceSecretChest").GetComponent<NomaiChest>();
        SecretSolution += secretChest.Open;

        yield return new WaitForEndOfFrame();

        _interface._upperOrb.SetOrbPosition(_interface._raisePillarSlot.transform.TransformPoint(new Vector3(0, 3.8f, 0)));
    }

    private int[] InvertCoord(int[] coord) => coord.Select(x => (10 - x) % 6).ToArray();

    public bool CheckCoords()
    {
        bool flag = _interface._nodeControllers[0].CheckCoordinate(_coordinateX);
        bool flag2 = _interface._nodeControllers[1].CheckCoordinate(_coordinateY);
        bool flag3 = _interface._nodeControllers[2].CheckCoordinate(_coordinateZ);

        return flag && flag2 && flag3;
    }

    public bool CheckInvertedCoords()
    {
        bool flag = _interface._nodeControllers[0].CheckCoordinate(InvertCoord(_coordinateY));
        bool flag2 = _interface._nodeControllers[1].CheckCoordinate(InvertCoord(_coordinateZ));
        bool flag3 = _interface._nodeControllers[2].CheckCoordinate(InvertCoord(_coordinateX));

        return flag && flag2 && flag3;
    }

    public void CheckAlternateSolution()
    {
        if (!_secretSolved && CheckInvertedCoords())
        {
            _secretSolved = true;
            SecretSolution?.Invoke();
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(NomaiCoordinateInterface), nameof(NomaiCoordinateInterface.CheckEyeCoordinates))]
    public static void NomaiCoordinateInterface_CheckEyeCoordinates(NomaiCoordinateInterface __instance, ref bool __result)
    {
        if (EscapeRoomJam4.InEscapeSystem())
        {
            // This way it disables raising it
            __result = _instance._secretSolved && _instance._wasSolved;
        }
    }

    public override bool IsSolved() => CheckCoords();
}

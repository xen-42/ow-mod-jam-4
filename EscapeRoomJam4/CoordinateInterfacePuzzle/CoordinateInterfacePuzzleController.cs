﻿using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
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

        _interface._raisePillarSlot.OnSlotActivated += (NomaiInterfaceSlot _) =>
        {
            foreach (var node in _interface._nodeControllers)
            {
                // Have to reset them else you get soft locked if its the eye coords
                node.ResetNodes();
            }
            _interface.SetPillarRaised(true, true);
        };
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

    //private int[] InvertCoord(int[] coord) => coord.Select(x => (10 - x) % 6).ToArray();
    private int[] RotateCoord(int[] coord) => coord.Select(x => (x + 3) % 6).ToArray();
    private int[] FlipCoord(int[] coords)
    {
        List<int> ints = new List<int>();
        foreach (int coord in coords)
        {
            int newInt = 0;
            switch (coord)
            {
                case 0:
                    newInt = 4;
                    break;
                case 1:
                    newInt = 3;
                    break;
                case 3:
                    newInt = 1;
                    break;
                case 4:
                    newInt = 0;
                    break;
                default:
                    newInt = coord;
                    break;
            }
            ints.Add(newInt);
        }
        return ints.ToArray();
    }

    public bool CheckCoords()
    {
        bool flag = _interface._nodeControllers[0].CheckCoordinate(_coordinateX);
        bool flag2 = _interface._nodeControllers[1].CheckCoordinate(_coordinateY);
        bool flag3 = _interface._nodeControllers[2].CheckCoordinate(_coordinateZ);

        return flag && flag2 && flag3;
    }

    public bool CheckAlternateCoords()
    {
        bool flag = _interface._nodeControllers[0].CheckCoordinate(FlipCoord(_coordinateX));
        bool flag2 = _interface._nodeControllers[1].CheckCoordinate(FlipCoord(_coordinateY));
        bool flag3 = _interface._nodeControllers[2].CheckCoordinate(FlipCoord(_coordinateZ));

        return flag && flag2 && flag3;
    }

    public void CheckAlternateSolution()
    {
        if (!_secretSolved && CheckAlternateCoords())
        {
            _secretSolved = true;
            SecretSolution?.Invoke();
            ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_INTERFACE_SECRET");
        }
    }

    public override bool IsSolved()
    {
        bool solved = CheckCoords();
        if (solved) ShipLogFactRevealer.instance.RevealFact("WYRM_XEN_JAM_4_INTERFACE_IDENTIFY");

        return solved;
    }
}

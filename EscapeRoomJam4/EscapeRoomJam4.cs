using EscapeRoomJam4.CoordinateInterfacePuzzle;
using EscapeRoomJam4.DockSignalPuzzle;
using EscapeRoomJam4.GhostPuzzle;
using EscapeRoomJam4.Jetpack;
using EscapeRoomJam4.LockAndKey;
using EscapeRoomJam4.ResourceHandling;
using EscapeRoomJam4.ScrollPuzzle;
using HarmonyLib;
using NewHorizons.Utility.Files;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class EscapeRoomJam4 : ModBehaviour
    {
        public static EscapeRoomJam4 Instance;
        public INewHorizons NewHorizons;

        public const string ESCAPE_SYSTEM = "xen.EscapeSystem";

        public static bool InEscapeSystem() => Instance.NewHorizons.GetCurrentStarSystem() == ESCAPE_SYSTEM;

        public void Awake()
        {
            Instance = this;
            // You won't be able to access OWML's mod helper in Awake.
            // So you probably don't want to do anything here.
            // Use Start() instead.
        }

        public void Start()
        {
            ModHelper.Console.WriteLine($"My mod {nameof(EscapeRoomJam4)} is loaded!", MessageType.Success);

            NewHorizons = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizons.LoadConfigs(this);
            NewHorizons.GetStarSystemLoadedEvent().AddListener(OnStarSystemLoaded);

            new Harmony("xen.EscapeRoomJam4").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void OnStarSystemLoaded(string system)
        {
            if (system == ESCAPE_SYSTEM)
            {
                // Quantum Puzzle
                var escapeShip = NewHorizons.GetPlanet("EscapeShip").transform;
                escapeShip.Find("Sector/QuantumPuzzle").gameObject.AddComponent<QuantumPuzzleController>();
                var scrollPuzzle = escapeShip.Find("Sector/ScrollPuzzle").gameObject.AddComponent<ScrollPuzzleController>();
                var scrollPuzzleChest = escapeShip.transform.Find("Sector/EscapeShip/Sector_Nomai/Geometry/ScrollPuzzleNomaiChest").GetComponent<NomaiChest>();
                scrollPuzzle.Solved.AddListener(scrollPuzzleChest.Open);

                var propulsionDisabledController = escapeShip.gameObject.AddComponent<PropulsionDisabledController>();
                var dreamworldSkyController = escapeShip.gameObject.AddComponent<DreamworldSkyController>();
                escapeShip.gameObject.AddComponent<PropulsionDisabledNotification>();
                escapeShip.gameObject.AddComponent<ResurrectionController>();
                escapeShip.gameObject.AddComponent<FuelReplenisher>();

                escapeShip.transform.Find("Sector/GhostBird").gameObject.AddComponent<GhostWalkController>();

                var coordPuzzle = escapeShip.transform.Find("Sector/VesselInterfacePuzzle").gameObject.AddComponent<CoordinateInterfacePuzzleController>();
                coordPuzzle.gameObject.AddComponent<ChromeToSandstoneReplacer>();

                // Has to wait at least one frame
                ModHelper.Events.Unity.FireOnNextUpdate(() =>
                {
                    dreamworldSkyController.TurnOn();
                    propulsionDisabledController.TurnOn();
                });

                var data = NewHorizons.QueryBody<LockAndKeyData>("EscapeShip", "$.extras.lockAndKey");
                if (data != null)
                {
                    BuildLockAndKeys.Make(NewHorizons.GetPlanet("EscapeShip"), data);
                }

                // Fix signals
                new GameObject(nameof(SignalSyncManager)).AddComponent<SignalSyncManager>();

                // Hide map mode lines to hide the hint better
                Transform mapModeRoot = GameObject.Find("Ship_Body").transform.Find("Module_Cabin/Systems_Cabin/ShipLogPivot/ShipLog/ShipLogPivot/ShipLogCanvas/MapMode/ScaleRoot/PanRoot");
                foreach (Transform planet in mapModeRoot.transform)
                {
                    foreach (Transform child in planet)
                    {
                        if (child.gameObject.name == "Line_ShipLog")
                        {
                            child.gameObject.SetActive(false);
                            break;
                        }
                    }
                }
            }
        }

        public static void WriteDebug(string line)
        {
#if DEBUG
            Instance.ModHelper.Console.WriteLine($"DEBUG: {line}", MessageType.Info);
#endif
        }

    }
}
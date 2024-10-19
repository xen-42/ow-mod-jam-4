using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;

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
                NewHorizons.GetPlanet("Test Planet").transform.Find("Sector/QuantumPuzzle").gameObject.AddComponent<ERQuantumPuzzleController>();
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
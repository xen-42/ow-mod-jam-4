using NewHorizons.Utility;
using OWML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EscapeRoomJam4
{
    public class SignalController : Puzzle
    {
        public static SignalController instance;

        private bool[] correctSignals = new bool[4];
        public List<AudioSignal> signals = new();

        public SignalName JammerSignal { get; private set; }
        public SignalFrequency Frequency { get; private set; }

        private bool _listeningForSignal;

        public Action UnlockAllSignals;

        private void Awake()
        {
            instance = this;

            if (!EnumUtils.TryParse("WYRM_XEN_JAM_4_JAMMER", out SignalName jammerSignal))
            {
                EscapeRoomJam4.Instance.ModHelper.Console.WriteLine("NO SIGNAL NAME?", OWML.Common.MessageType.Error);
            }
            else
            {
                JammerSignal = jammerSignal;
            }

            if (!EnumUtils.TryParse("WYRM_XEN_JAM_4_SIGNAL", out SignalFrequency frequency))
            {
                EscapeRoomJam4.Instance.ModHelper.Console.WriteLine("NO SIGNAL FREQUENCY?", OWML.Common.MessageType.Error);
            }
            else
            {
                Frequency = frequency;
            }

            // Can't use Locator.GetAudioSignal here because they arent registered yet
            signals = GameObject.FindObjectsOfType<AudioSignal>().Where(x => x._frequency == Frequency).ToList();

            // Once the jammer signal is discovered, discover all the other ones automatically
            // Annoyingly this doesnt effect the inactive ones for some reason, so those get learned when the button is pressed to cycle them
            if (!PlayerData.KnowsSignal(JammerSignal))
            {
                _listeningForSignal = true;
                GlobalMessenger.AddListener("IdentifySignal", OnIdentifySignal);
            }
        }

        public void ResetSignalscope()
        {
            if (Locator.GetToolModeSwapper().GetToolMode() == ToolMode.SignalScope)
            {
                Locator.GetToolModeSwapper().UnequipTool();

                EscapeRoomJam4.Instance.ModHelper.Events.Unity.FireInNUpdates(() =>
                {
                    Locator.GetToolModeSwapper().EquipToolMode(ToolMode.SignalScope);
                }, 2);
            }
        }

        private void OnIdentifySignal()
        {
            if (PlayerData.KnowsSignal(JammerSignal))
            {
                _listeningForSignal = false;
                GlobalMessenger.RemoveListener("IdentifySignal", OnIdentifySignal);
                foreach (var signal in signals)
                {
                    PlayerData.LearnSignal(signal._name);
                }
                UnlockAllSignals?.Invoke();
                ResetSignalscope();
            }
        }

        public void OnDestroy()
        {
            if (_listeningForSignal)
            {
                GlobalMessenger.RemoveListener("IdentifySignal", OnIdentifySignal);
            }
        }

        public void ChangeCorrectFlag(int id, bool correct)
        {
            correctSignals[id] = correct;
            CheckIfSolved();
        }

        public override bool IsSolved()
        {
            foreach (bool isCorrect in correctSignals)
            {
                if (!isCorrect) return false;
            }
            return true;
        }
    }
}

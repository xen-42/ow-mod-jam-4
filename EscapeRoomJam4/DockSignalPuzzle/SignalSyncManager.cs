using OWML.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeRoomJam4.DockSignalPuzzle;

/// <summary>
/// Adapted from TravelerController
/// </summary>
internal class SignalSyncManager : MonoBehaviour
{
    private List<AudioSignal> _signals = new List<AudioSignal>(16);

    private AudioSignal _mainSignal;

    public static SignalSyncManager instance;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        foreach (var signal in SignalController.instance.signals)
        {
            if (signal.GetFrequency() == SignalController.instance.Frequency)
            {
                _signals.Add(signal);
                if (signal.GetName() == SignalController.instance.JammerSignal)
                {
                    _mainSignal = signal;
                }
            }
        }

        GlobalMessenger.AddListener("GameUnpaused", new Callback(OnUnpause));
        GlobalMessenger.AddListener("EndFastForward", new Callback(OnEndFastForward));
        GlobalMessenger<Signalscope>.AddListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
    }

    public void OnDestroy()
    {
        GlobalMessenger.RemoveListener("GameUnpaused", new Callback(OnUnpause));
        GlobalMessenger.RemoveListener("EndFastForward", new Callback(OnEndFastForward));
        GlobalMessenger<Signalscope>.RemoveListener("EquipSignalscope", new Callback<Signalscope>(OnEquipSignalscope));
    }

    private void OnUnpause()
    {
        foreach (var signal in _signals)
        {
            var isPlaying = signal.GetOWAudioSource().isPlaying;
            signal.GetOWAudioSource().Stop();
            if (isPlaying)
            {
                signal.GetOWAudioSource().Play();
                signal.GetOWAudioSource().timeSamples = 0;
            }
        }
    }

    private void OnEndFastForward()
    {
        this.OnUnpause();
    }

    private void OnEquipSignalscope(Signalscope scope)
    {
        this.Sync();
    }

    public void Sync()
    {
        foreach (var signal in _signals)
        {
            if (signal == _mainSignal) continue;
            if (signal.IsOnlyAudibleToScope() && !signal.GetOWAudioSource().isPlaying)
            {
                signal.GetOWAudioSource().SetLocalVolume(0f);
                signal.GetOWAudioSource().Play();
            }
            signal.GetOWAudioSource().timeSamples = _mainSignal.GetOWAudioSource().timeSamples;
        }
    }
}

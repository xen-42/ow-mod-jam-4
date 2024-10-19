using HarmonyLib;
using System.Linq;

namespace EscapeRoomJam4.QuantumPuzzle;

[HarmonyPatch(typeof(SocketedQuantumObject))]
public static class SocketedQuantumObjectPatches
{
    private static bool _skipPatch;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SocketedQuantumObject), nameof(SocketedQuantumObject.MoveToSocket))]
    private static bool SocketedQuantumObject_MoveToSocket(SocketedQuantumObject __instance, QuantumSocket socket)
    {
        if (_skipPatch)
        {
            return true;
        }

        if (EscapeRoomJam4.InEscapeSystem())
        {
            if (socket.GetVisibilityObject() is SnapshotLockableVisibilityObject qObj)
            {
                // Do not allow it to switch to a socket that is photographed
                // Instead try to swap with an occupied socket
                if (qObj.IsLockedByProbeSnapshot())
                {
                    _skipPatch = true;
                    // Try to swap
                    var swapSocket = __instance._socketList.FirstOrDefault(x => x._quantumObject != null && x._quantumObject != __instance && !x._quantumObject.IsLocked());
                    if (swapSocket != null)
                    {
                        var originalSocket = __instance.GetCurrentSocket();
                        var otherQObj = swapSocket._quantumObject as SocketedQuantumObject;
                        otherQObj.MoveToSocket(socket);
                        __instance.MoveToSocket(swapSocket);
                        otherQObj.MoveToSocket(originalSocket);
                    }
                    else
                    {
                        __instance.MoveToSocket(__instance.GetCurrentSocket());
                    }

                    _skipPatch = false;
                    return false;
                }
            }
        }

        return true;
    }
}

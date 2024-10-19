namespace EscapeRoomJam4.QuantumPuzzle;

/// <summary>
/// A quantum object that does nothing but track if its been photographed
/// </summary>
internal class SnapshotLockableVisibilityObject : QuantumObject
{
    public override bool ChangeQuantumState(bool skipInstantVisibilityCheck) => true;
}

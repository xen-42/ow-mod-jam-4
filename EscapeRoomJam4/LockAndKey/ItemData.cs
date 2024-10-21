using UnityEngine;

namespace EscapeRoomJam4.LockAndKey;

public class ItemData
{
    public string itemType;
    public string name;
    public Vector3 position;
    public Vector3 rotation;
    public float scale;
    public string assetBundle;
    public string path;

    /// <summary>
    /// Only used for keys
    /// </summary>
    public string boxPath;
}

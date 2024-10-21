using UnityEngine;

namespace EscapeRoomJam4.LockAndKey;

public static class BuildLockAndKeys
{
    public static void Make(GameObject planetGO, LockAndKeyData data)
    {
        var sector = planetGO.GetComponentInChildren<Sector>();
        foreach (var lockData in data.locks)
        {
            var socketGO = NewHorizons.Builder.Props.DetailBuilder.Make(planetGO, sector, EscapeRoomJam4.Instance, new()
            {
                rename = lockData.name,
                position = lockData.position,
                rotation = lockData.rotation,
                itemSocket = new()
                {
                    itemType = lockData.itemType,
                    interactRange = 4,
                    isRelativeToParent = true,
                    colliderRadius = 0.5f
                }
            });
            var socketVisual = NewHorizons.Builder.Props.DetailBuilder.Make(planetGO, sector, EscapeRoomJam4.Instance, new()
            {
                rename = lockData.name + "Geo",
                assetBundle = lockData.assetBundle,
                path = lockData.path,
                scale = lockData.scale,
            });
            socketVisual.transform.parent = socketGO.transform;
            socketVisual.transform.localPosition = Vector3.zero;
            socketVisual.transform.localRotation = Quaternion.identity;

            // Once a key is placed into a lock it cannot be removed
            var socket = socketGO.GetComponentInChildren<OWItemSocket>();
            socket.OnSocketablePlaced += (OWItem item) =>
            {
                socket.EnableInteraction(false);
            };
        }
        foreach (var keyData in data.keys)
        {
            var parentToBox = !string.IsNullOrEmpty(keyData.boxPath);
            var itemGO = NewHorizons.Builder.Props.DetailBuilder.Make(planetGO, sector, EscapeRoomJam4.Instance, new()
            {
                rename = keyData.name,
                position = parentToBox ? new Vector3(0, 0.054f, -0.74f) : keyData.position,
                rotation = parentToBox ? new Vector3(0, 180, 180) : keyData.rotation,
                item = new()
                {
                    itemType = keyData.itemType,
                    interactRange = 4,
                    dropNormal = new(0, 0, 1),
                    dropOffset = new(0, 0, 0.1f),
                    colliderRadius = 0.5f
                },
                parentPath = keyData.boxPath,
                isRelativeToParent = parentToBox
            });
            var itemVisual = NewHorizons.Builder.Props.DetailBuilder.Make(planetGO, sector, EscapeRoomJam4.Instance, new()
            {
                rename = keyData.name + "Geo",
                assetBundle = keyData.assetBundle,
                path = keyData.path,
                scale = keyData.scale,
            });
            itemVisual.transform.parent = itemGO.transform;
            itemVisual.transform.localPosition = Vector3.zero;
            itemVisual.transform.localRotation = Quaternion.identity;
        }
    }
}

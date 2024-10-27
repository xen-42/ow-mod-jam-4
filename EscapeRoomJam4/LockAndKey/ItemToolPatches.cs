using HarmonyLib;
using NewHorizons.Components.Props;

namespace EscapeRoomJam4.LockAndKey;

[HarmonyPatch]
public static class ItemToolPatches
{
    [HarmonyPrefix, HarmonyPatch(typeof(ItemTool), nameof(ItemTool.DropItem))]
    public static void ItemTool_DropItem(ItemTool __instance)
    {
        if (EscapeRoomJam4.InEscapeSystem() && __instance.GetHeldItem() is NHItem)
        {
            Locator.GetPlayerAudioController().PlayDropItem(ItemType.SharedStone);
        }
    }
}

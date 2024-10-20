using HarmonyLib;

namespace EscapeRoomJam4.ScrollPuzzle;

[HarmonyPatch(typeof(ItemTool))]
public static class ItemToolPatches
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(ItemTool.UpdateState))]
    public static bool ItemTool_UpdateState(ItemTool __instance, ItemTool.PromptState newState, string itemName)
    {
        if (EscapeRoomJam4.InEscapeSystem()) 
        {
            if (__instance._promptState != newState && newState == ItemTool.PromptState.SOCKET && Locator.GetToolModeSwapper().GetItemCarryTool().GetHeldItemType() == ItemType.Scroll)
            {
                var focusedSocket = Locator.GetToolModeSwapper()._firstPersonManipulator._focusedItemSocket;

                if (focusedSocket != null)
                {
                    var colour = string.Empty;
                    if (focusedSocket == ScrollPuzzleController.blueSocket)
                    {
                        colour = EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("BLUE_SOCKET");
                    }
                    else if (focusedSocket == ScrollPuzzleController.greenSocket)
                    {
                        colour = EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("GREEN_SOCKET");
                    }
                    else if (focusedSocket == ScrollPuzzleController.redSocket)
                    {
                        colour = EscapeRoomJam4.Instance.NewHorizons.GetTranslationForOtherText("RED_SOCKET");
                    }

                    if (!string.IsNullOrEmpty(colour))
                    {
                        __instance._promptState = newState;
                        var text2 = UITextLibrary.GetString(UITextType.ItemInsertPrompt) + itemName + " " + colour;
                        __instance._interactButtonPrompt.SetText(text2);
                        __instance._interactButtonPrompt.SetVisibility(true);
                        __instance._cancelButtonPrompt.SetVisibility(false);
                        __instance._messageOnlyPrompt.SetVisibility(false);

                        return false;
                    }
                }
            }
        }
        return true;
    }
}

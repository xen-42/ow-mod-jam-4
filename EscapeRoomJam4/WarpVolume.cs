using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4;

/// <summary>
/// Must be on a GameObject on the BasicEffectVolume layer, with a collider component with isTrigger = true
/// </summary>
public class WarpVolume : MonoBehaviour
{
    private PlayerCameraEffectController _effectController;

    [SerializeField]
    private Transform _respawnPoint;

    private bool isWarping = false;

    public void Start()
    {
        _effectController = Locator.GetPlayerCamera().GetComponent<PlayerCameraEffectController>();
    }

    public void OnTriggerEnter(Collider hitCollider)
    {
        if (!isWarping && hitCollider.attachedRigidbody.gameObject == Locator.GetPlayerBody()._rigidbody.gameObject)
        {
            StartCoroutine(WarpPlayer());
        }
    }

    private IEnumerator WarpPlayer()
    {
        isWarping = true;
        _effectController.CloseEyes(1f);
        OWInput.ChangeInputMode(InputMode.None);
        ReticleController.Hide();
        Locator.GetPromptManager().SetPromptsVisible(false);

        yield return new WaitForSeconds(1f);

        OWInput.ChangeInputMode(InputMode.Character);

        _effectController.OpenEyes(1f);
        ReticleController.Show();
        Locator.GetPromptManager().SetPromptsVisible(true);
        Locator.GetPlayerBody().GetComponent<PlayerBody>().WarpToPositionRotation(_respawnPoint.position, _respawnPoint.rotation);
        isWarping = false;
    }
}

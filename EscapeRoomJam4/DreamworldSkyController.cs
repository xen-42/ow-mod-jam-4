using UnityEngine;

namespace EscapeRoomJam4;

public class DreamworldSkyController : MonoBehaviour
{
    private GameObject _dreamworldAtmosphere;

    public void Awake()
    {
        _dreamworldAtmosphere = transform.Find("Sector/Atmosphere_Dreamworld").gameObject;

        var dawnController = _dreamworldAtmosphere.GetComponentInChildren<DreamWorldDawnController>();
        dawnController.enabled = false;
        dawnController._renderer.SetColor(dawnController._baseColor);
        Component.DestroyImmediate(dawnController);

        _dreamworldAtmosphere.SetActive(false);
    }

    public void TurnOn()
    {
        _dreamworldAtmosphere.SetActive(true);
    }

    public void TurnOff()
    {
        _dreamworldAtmosphere.SetActive(false);
    }
}

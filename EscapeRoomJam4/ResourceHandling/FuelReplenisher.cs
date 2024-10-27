using System.Collections;
using UnityEngine;

namespace EscapeRoomJam4.ResourceHandling;

public class FuelReplenisher : MonoBehaviour
{
    private PlayerResources _resources;

    public void Start()
    {
        _resources = GameObject.FindObjectOfType<PlayerResources>();
    }

    public void Update()
    {
        if (!_resources.IsRefueling() && _resources.GetFuel() < _resources.GetLowFuel())
        {
            _resources.StartRefillResources(true, false);
            StartCoroutine(StopRefillCoroutine());
        }
    }

    private IEnumerator StopRefillCoroutine()
    {
        yield return new WaitForSeconds(5f);
        _resources.StopRefillResources();
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncedToggle : UdonSharpBehaviour
{
    [UdonSynced] private bool isActive = false; // synced variable
    public GameObject targetObject;            // object to toggle

    void Start()
    {
        UpdateObjectState();
    }

    // Call this from another script or UdonBehaviour
    public void ToggleObject()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        isActive = !isActive;
        UpdateObjectState();
        RequestSerialization();
    }

    // Optional: call to force ON
    public void SetObjectOn()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        isActive = true;
        UpdateObjectState();
        RequestSerialization();
    }

    // Optional: call to force OFF
    public void SetObjectOff()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        isActive = false;
        UpdateObjectState();
        RequestSerialization();
    }

    private void UpdateObjectState()
    {
        if (targetObject != null)
            targetObject.SetActive(isActive);
    }
}

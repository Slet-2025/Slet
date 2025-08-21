using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LocalObjectToggle : UdonSharpBehaviour
{
    [Header("Object to toggle locally")]
    public GameObject targetObject;

    // Call this to show the object only for the local player
    public void ShowObject()
    {
        if (targetObject != null)
        {
            SetObjectActive(true);
        }
    }

    // Call this to hide the object only for the local player
    public void HideObject()
    {
        if (targetObject != null)
        {
            SetObjectActive(false);
        }
    }

    private void SetObjectActive(bool state)
    {
        // Option 1: Enable/disable the GameObject itself
        targetObject.SetActive(state);

        // Option 2: (Alternative) Disable/enable all Renderers only
        // Renderer[] renderers = targetObject.GetComponentsInChildren<Renderer>(true);
        // foreach (Renderer rend in renderers)
        // {
        //     rend.enabled = state;
        // }
    }
}


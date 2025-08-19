using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class UIButtonUdon : UdonSharpBehaviour
{
    [Header("First target script to trigger")]
    public UdonSharpBehaviour targetScript1; // Drag your first script here
    public string targetEventName1 = "NextPicture"; // First method name

    [Header("Second target script to trigger")]
    public UdonSharpBehaviour targetScript2; // Drag your second script here
    public string targetEventName2 = "OtherMethod"; // Second method name

    public override void Interact()
    {
        Debug.Log("Button clicked!");

        if (targetScript1 != null && !string.IsNullOrEmpty(targetEventName1))
        {
            targetScript1.SendCustomEvent(targetEventName1);
        }

        if (targetScript2 != null && !string.IsNullOrEmpty(targetEventName2))
        {
            targetScript2.SendCustomEvent(targetEventName2);
        }
    }
}

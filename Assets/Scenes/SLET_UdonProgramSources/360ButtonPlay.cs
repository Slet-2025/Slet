using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class VrButtonPlay : UdonSharpBehaviour
{
    [Header("First target script to trigger")]
    public UdonSharpBehaviour targetScript1; // Drag your first script here
    public string targetEventName1 = "NextPicture"; // First method name

    [Header("Objects to hide locally")]
    public GameObject[] objectsToHide; // Drag objects here

    [Header("Optional: duration to hide objects (seconds)")]
    public float hideDuration = 10f; // Set the length of your video/audio

    private bool isHiding = false;
    private float hideEndTime = 0f;

    public override void Interact()
    {
        Debug.Log("Button clicked!");

        // Trigger the first Udon script method
        if (targetScript1 != null && !string.IsNullOrEmpty(targetEventName1))
        {
            targetScript1.SendCustomEvent(targetEventName1);
        }

        // Hide all assigned objects locally
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Start timer to restore objects
        if (hideDuration > 0f)
        {
            isHiding = true;
            hideEndTime = Time.time + hideDuration;
        }
    }

    private void Update()
    {
        if (isHiding && Time.time >= hideEndTime)
        {
            // Restore all objects
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }

            isHiding = false;
        }
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ButtonPlaySound : UdonSharpBehaviour
{
    public AudioSource audioSource; // Drag your AudioSource here in the Inspector

    public override void OnPickupUseDown()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Use button pressed -> Play audio");
        }
    }

    public override void OnPickupUseUp()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            Debug.Log("Use button released -> Stop audio");
        }
    }
}

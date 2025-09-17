using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ButtonPlaySound : UdonSharpBehaviour
{
    public AudioSource audioSource; // Attach this to the pickup object with Spatial Blend = 1 (3D sound)

    public override void OnPickupUseDown()
    {
        // Call PlaySoundGlobal() on all clients
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(PlaySoundGlobal));
    }

    public override void OnPickupUseUp()
    {
        // Call StopSoundGlobal() on all clients
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, nameof(StopSoundGlobal));
    }

    public void PlaySoundGlobal()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // everyone will hear it from the object's position
        }
    }

    public void StopSoundGlobal()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

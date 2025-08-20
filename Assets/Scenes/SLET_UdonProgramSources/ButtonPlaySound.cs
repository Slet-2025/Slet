using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ButtonPlaySound : UdonSharpBehaviour
{
    private AudioSource audioSource;

    [Header("Custom Settings")]
    public float playDuration = 0f; // 0 = play full clip, otherwise stop after seconds

    void Start()
    {
        // Get AudioSource from same GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("No AudioSource found on this object!");
        }
    }

    public override void Interact()
    {
        if (audioSource == null) return;

        // Play from the beginning
        audioSource.Stop();
        audioSource.Play();

        // If playDuration > 0, stop playback after that many seconds
        if (playDuration > 0f)
        {
            SendCustomEventDelayedSeconds(nameof(StopSound), playDuration);
        }
    }

    public void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}

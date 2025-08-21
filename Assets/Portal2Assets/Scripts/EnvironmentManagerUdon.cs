using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
public class EnvironmentManagerUdon : UdonSharpBehaviour
{
    [Header("Book reference")]
    public Book book;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip globalTriggerSound;

    [Header("Environment triggers")]
    public PageEnvironmentTriggerLocal[] triggers;

    private int lastTriggeredPage = -1;

    public void OnPageFlipped()
    {
        int currentPage = book.currentPage;

        foreach (PageEnvironmentTriggerLocal trigger in triggers)
        {
            if (!trigger.isActive) continue;

            if (trigger.pageNumber == currentPage)
            {
                if (lastTriggeredPage == trigger.pageNumber)
                {
                    return; // Already triggered
                }

                ApplyTriggerLocal(trigger);
                lastTriggeredPage = trigger.pageNumber;
                break;
            }
        }
    }

    private void ApplyTriggerLocal(PageEnvironmentTriggerLocal trigger)
    {
        // Activate objects locally
        foreach (GameObject obj in trigger.activateObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        // Deactivate objects locally
        foreach (GameObject obj in trigger.deactivateObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Swap textures locally
        foreach (TextureSwapLocal swap in trigger.textureSwaps)
        {
            if (swap.newTexture == null) continue;

            if (swap.targetRenderer != null)
            {
                swap.targetRenderer.material.mainTexture = swap.newTexture;
            }
            else if (swap.targetMaterial != null)
            {
                swap.targetMaterial.mainTexture = swap.newTexture;
            }
        }

        // Play sound locally
        if (trigger.playSound && globalTriggerSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(globalTriggerSound);
        }
    }

    [System.Serializable]
    public class TextureSwapLocal
    {
        public Renderer targetRenderer;
        public Material targetMaterial;
        public Texture newTexture;
    }

    [System.Serializable]
    public class PageEnvironmentTriggerLocal
    {
        public string note;
        public bool isActive = true;
        public int pageNumber;
        public GameObject[] activateObjects;
        public GameObject[] deactivateObjects;
        public TextureSwapLocal[] textureSwaps;
        public bool playSound = true;
    }
}

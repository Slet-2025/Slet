using UdonSharp;
using UnityEngine;
using VRC.Udon;

public class EnvironmentControllerLocal : UdonSharpBehaviour
{
    [Header("Objects to control")]
    public string triggerInfo;

    [Header("Objects to control")]
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    [Header("Texture swapping 1")]
    public Renderer targetRenderer1;   // optional
    public Material targetMaterial1;   // optional
    public Texture newTexture1;        // texture to apply

    [Header("Texture swapping 2")]
    public Renderer targetRenderer2;   // optional
    public Material targetMaterial2;   // optional
    public Texture newTexture2;        // texture to apply

    [Header("Texture swapping 3")]
    public Renderer targetRenderer3;   // optional
    public Material targetMaterial3;   // optional
    public Texture newTexture3;        // texture to apply

    // --- Activate objects ---
    public void ActivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log("Activating: " + obj.name);
            }
        }
    }

    // --- Deactivate objects ---
    public void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                Debug.Log("Deactivating: " + obj.name);
            }
        }
    }

    // --- Swap texture 1 ---
    public void SwapTexture1()
    {
        if (newTexture1 == null) return;

        if (targetRenderer1 != null)
        {
            foreach (Material mat in targetRenderer1.materials)
            {
                if (mat != null) mat.mainTexture = newTexture1;
            }
        }
        else if (targetMaterial1 != null)
        {
            targetMaterial1.mainTexture = newTexture1;
        }

        Debug.Log("Swapped texture 1");
    }

    // --- Swap texture 2 ---
    public void SwapTexture2()
    {
        if (newTexture2 == null) return;

        if (targetRenderer2 != null)
        {
            foreach (Material mat in targetRenderer2.materials)
            {
                if (mat != null) mat.mainTexture = newTexture2;
            }
        }
        else if (targetMaterial2 != null)
        {
            targetMaterial2.mainTexture = newTexture2;
        }

        Debug.Log("Swapped texture 2");
    }

    // --- Swap texture 3 ---
    public void SwapTexture3()
    {
        if (newTexture3 == null) return;

        if (targetRenderer3 != null)
        {
            foreach (Material mat in targetRenderer3.materials)
            {
                if (mat != null) mat.mainTexture = newTexture3;
            }
        }
        else if (targetMaterial3 != null)
        {
            targetMaterial3.mainTexture = newTexture3;
        }

        Debug.Log("Swapped texture 3");
    }

    // --- Execute all actions at once ---
    public void ExecuteAll()
    {
        ActivateObjects();
        DeactivateObjects();
        SwapTexture1();
        SwapTexture2();
        SwapTexture3();
        Debug.Log("All executed!");
    }
}

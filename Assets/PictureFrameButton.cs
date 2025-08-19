/* Pachy Picture Frame (Modified)
 * Button-based version (no timer)
 * by pachipon@VRC, modified for manual control
 */

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PictureFrameButton : UdonSharpBehaviour
{
    public Texture[] pictureList;          // List of textures to cycle through
    private int[] shuffleOrder;
    public bool shufflePictures = false;   // Whether to shuffle pictures
    public Material screenMaterial;        // Target screen material

    private MeshRenderer screenRenderer;
    private string screenName;
    private int pictureIndex = 0;
    private int pictureCount = -1;

    [UdonSynced] private int sync_pictureIndex = 0;

    void Start()
    {
        screenName = screenMaterial.name;
        screenRenderer = gameObject.GetComponent<MeshRenderer>();
        shuffleOrder = ShuffleList(pictureList);

        // Ensure first picture is shown only by Master
        if (Networking.IsMaster)
        {
            PictureChange();
            RequestSerialization();
        }
    }

    /// <summary>
    /// Called by button click (hook this via UnityEvent → UdonBehaviour → SendCustomEvent).
    /// Must be called by Master.
    /// </summary>
    public void NextPicture()
    {
        if (Networking.IsMaster)
        {
            PictureChange();
            RequestSerialization();
        }
    }

    /// <summary>
    /// Changes the displayed picture. Must be called by Master.
    /// </summary>
    public void PictureChange()
    {
        if (++pictureCount >= pictureList.Length)
        {
            pictureCount = 0;
        }

        if (shufflePictures)
        {
            pictureIndex = shuffleOrder[pictureCount];
        }
        else
        {
            pictureIndex = pictureCount;
        }

        sync_pictureIndex = pictureIndex;

        Material m = GetScreenMaterial();
        if (m) { m.mainTexture = pictureList[pictureIndex]; }
    }

    /// <summary>
    /// Non-masters update when synced variable changes.
    /// </summary>
    public override void OnDeserialization()
    {
        if (sync_pictureIndex != pictureIndex)
        {
            PictureChangeDeserialize();
        }
    }

    private void PictureChangeDeserialize()
    {
        pictureIndex = sync_pictureIndex;
        Material m = GetScreenMaterial();
        if (m) { m.mainTexture = pictureList[pictureIndex]; }
    }

    /// <summary>
    /// Find screen material
    /// </summary>
    private Material GetScreenMaterial()
    {
        Material[] rendererMaterials = screenRenderer.materials;
        foreach (Material mat in rendererMaterials)
        {
            if (mat.name.Equals(screenName + " (Instance)"))
            {
                return mat;
            }
        }
        return null;
    }

    private int[] ShuffleList(Texture[] textures)
    {
        int[] shuffle = new int[textures.Length];
        for (int x = 0; x < shuffle.Length; x++)
        {
            shuffle[x] = x;
        }

        int temp;
        for (int i = 0; i < shuffle.Length - 2; i++)
        {
            temp = shuffle[i];
            int j = Random.Range(i, shuffle.Length);
            shuffle[i] = shuffle[j];
            shuffle[j] = temp;
        }
        return shuffle;
    }
}

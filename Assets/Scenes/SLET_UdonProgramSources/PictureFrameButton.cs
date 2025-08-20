/* Button-based Picture Frame with Next/Previous + PictureChange wrapper */

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PictureFrameButton : UdonSharpBehaviour
{
    public Texture[] pictureList;
    public bool shufflePictures = false;
    public Material screenMaterial;

    private MeshRenderer screenRenderer;
    private string screenName;
    private int pictureIndex = 0;
    private int pictureCount = -1;
    private int[] shuffleOrder;

    [UdonSynced] private int sync_pictureIndex = 0;

    void Start()
    {
        if (screenMaterial == null || pictureList == null || pictureList.Length == 0) return;

        screenName = screenMaterial.name;
        screenRenderer = GetComponent<MeshRenderer>();
        shuffleOrder = ShuffleList(pictureList);

        if (Networking.IsMaster)
        {
            PictureChange();         // wrapper exists again
            RequestSerialization();
        }
    }

    // --- Public events you can call from buttons ---
    public void NextPicture()
    {
        if (Networking.IsMaster) StepPicture(1);
    }

    public void PreviousPicture()
    {
        if (Networking.IsMaster) StepPicture(-1);
    }

    // Back-compat wrapper so old references don’t break
    public void PictureChange()
    {
        StepPicture(1);
    }

    // --- Core logic ---
    private void StepPicture(int step)
    {
        if (pictureList == null || pictureList.Length == 0) return;

        pictureCount += step;
        if (pictureCount >= pictureList.Length) pictureCount = 0;
        if (pictureCount < 0) pictureCount = pictureList.Length - 1;

        pictureIndex = shufflePictures ? shuffleOrder[pictureCount] : pictureCount;
        sync_pictureIndex = pictureIndex;

        Material m = GetScreenMaterial();
        if (m != null) m.mainTexture = pictureList[pictureIndex];

        RequestSerialization();
    }

    public override void OnDeserialization()
    {
        if (sync_pictureIndex != pictureIndex)
        {
            pictureIndex = sync_pictureIndex;
            Material m = GetScreenMaterial();
            if (m != null) m.mainTexture = pictureList[pictureIndex];
        }
    }

    private Material GetScreenMaterial()
    {
        if (screenRenderer == null) return null;
        var mats = screenRenderer.materials;
        string target = screenName + " (Instance)";
        for (int i = 0; i < mats.Length; i++)
            if (mats[i] != null && mats[i].name == target) return mats[i];
        return null;
    }

    private int[] ShuffleList(Texture[] textures)
    {
        int len = textures.Length;
        int[] order = new int[len];
        for (int i = 0; i < len; i++) order[i] = i;

        for (int i = 0; i < len - 2; i++)
        {
            int j = Random.Range(i, len);
            int tmp = order[i];
            order[i] = order[j];
            order[j] = tmp;
        }
        return order;
    }
}

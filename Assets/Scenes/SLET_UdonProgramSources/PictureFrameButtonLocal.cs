using UdonSharp;
using UnityEngine;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class PictureFrameButtonLocal : UdonSharpBehaviour
{
    [Header("Picture Settings")]
    public Texture[] pictureList;
    public bool shufflePictures = false;
    public Material screenMaterial;

    [Header("Page Event Settings")]
    public int[] eventPages;                       // Page numbers
    public UdonSharpBehaviour[] eventScripts;      // Corresponding scripts
    public string[] eventMethods;                  // Corresponding method names

    private MeshRenderer screenRenderer;
    private string screenName;
    private int pictureIndex = 0;
    private int pictureCount = -1;
    private int[] shuffleOrder;

    void Start()
    {
        if (screenMaterial == null || pictureList == null || pictureList.Length == 0) return;

        screenRenderer = GetComponent<MeshRenderer>();
        screenName = screenMaterial.name;
        shuffleOrder = ShuffleList(pictureList);

        StepPicture(1);
    }

    public void NextPicture() => StepPicture(1);
    public void PreviousPicture() => StepPicture(-1);

    private void StepPicture(int step)
    {
        if (pictureList == null || pictureList.Length == 0) return;

        pictureCount += step;

        if (pictureCount >= pictureList.Length) pictureCount = 0;
        if (pictureCount < 0) pictureCount = pictureList.Length - 1;

        pictureIndex = shufflePictures ? shuffleOrder[pictureCount] : pictureCount;

        Material m = GetScreenMaterial();
        if (m != null) m.mainTexture = pictureList[pictureIndex];
        Debug.Log("picture count = " + pictureCount);
        TriggerPageEvents(pictureCount);
    }

    private void TriggerPageEvents(int page)
    {
        for (int i = 0; i < eventPages.Length; i++)
        {
            if (eventPages[i] == page && eventScripts[i] != null && !string.IsNullOrEmpty(eventMethods[i]))
            {
                Debug.Log("page found!");
                eventScripts[i].SendCustomEvent(eventMethods[i]);
            }
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

using UdonSharp;
using UnityEngine;
using VRC.Udon;

public class ZineControllerUdon : UdonSharpBehaviour
{
    [Header("Zine Components")]
    public Animator zineAnimator;
    public GameObject zinePages;
    public GameObject zineParticles;
    public GameObject btn_open;
    public GameObject btn_close;
    public GameObject RightNext;
    public GameObject zineButtons;

    [Header("State")]
    private bool isOpened = false; // Local-only

    private void Start()
    {
        ApplyVisuals();
    }

    /// <summary>
    /// Open the zine locally
    /// </summary>
    public void OpenZine()
    {
        isOpened = true;
        ApplyVisuals();
    }

    /// <summary>
    /// Close the zine locally
    /// </summary>
    public void CloseZine()
    {
        isOpened = false;
        ApplyVisuals();
    }

    /// <summary>
    /// Apply visual changes based on isOpened
    /// </summary>
    private void ApplyVisuals()
    {
        if (zineAnimator != null)
            zineAnimator.SetBool("isOpened", isOpened);

        if (zinePages != null)
            zinePages.SetActive(isOpened);

        if (zineParticles != null)
            zineParticles.SetActive(!isOpened);

        if (btn_open != null)
            btn_open.SetActive(!isOpened);

        if (btn_close != null)
            btn_close.SetActive(isOpened);

        if (RightNext != null)
            RightNext.SetActive(!isOpened);

        if (zineButtons != null)
            zineButtons.SetActive(isOpened);
    }
}

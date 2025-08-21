using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TeleportPlayer : UdonSharpBehaviour
{
    [Header("Target Position for Teleport")]
    public Transform targetTransform; // Assign the position/rotation to teleport to

    // Public method to teleport the local player
    public void TeleportNow()
    {
        if (targetTransform == null) return;

        // Teleport the local player
        Networking.LocalPlayer.TeleportTo(
            targetTransform.position,
            targetTransform.rotation
        );

        Debug.Log("Player teleported to: " + targetTransform.position);
    }
}

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class SimpleSwimVolume : UdonSharpBehaviour
{
    public float waterLevelY;

    [Header("Movement")]
    public float gravityInWater = 0.2f;
    public float verticalSpeed = 2.0f;
    public float swimWalk = 2f, swimRun = 2f, swimStrafe = 2f;

    [Header("Surface assist")]
    public float surfaceBand = 0.30f;     // meters below the surface where we help you float
    public float headOffset = 0.05f;      // keep head just under the surface
    public float buoyancyStrength = 8f;   // how strongly we push toward the surface (tune)
    public float maxSurfaceVel = 2.5f;    // cap vertical speed from buoyancy
    public float diveBypass = 0.35f;      // seconds: hold Descend to break the surface stickiness

    private VRCPlayerApi local;
    private bool inWater;
    private float bypassUntil;

    void Start() { local = Networking.LocalPlayer; }

    void Update()
    {
        if (local == null || !inWater) return;

        // inputs
        bool ascend = Input.GetButton("Jump");
        bool descend = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        var headY = local.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position.y;
        float diff = waterLevelY - (headY + headOffset); // >0 = head under surface a bit
        bool nearSurface = diff >= 0f && diff <= surfaceBand;

        var vel = local.GetVelocity();

        // Manual up/down
        if (ascend) vel.y = verticalSpeed;
        else if (descend) { vel.y = -verticalSpeed; bypassUntil = Time.time + diveBypass; }
        else
        {
            // Surface stickiness (if not actively diving/ascending)
            if (nearSurface && Time.time > bypassUntil)
            {
                float targetVy = Mathf.Clamp(diff * buoyancyStrength, -maxSurfaceVel, maxSurfaceVel);
                vel.y = Mathf.Lerp(vel.y, targetVy, 0.15f); // smooth toward hover
            }
            else
            {
                // Deeper water: damp falling a bit so it feels buoyant
                if (vel.y < 0f) vel.y *= 0.9f;
            }
        }

        local.SetVelocity(vel);
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    { if (player.isLocal) EnterWater(); }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    { if (player.isLocal) ExitWater(); }

    private void EnterWater()
    {
        inWater = true;
        bypassUntil = 0f;
        local.SetGravityStrength(gravityInWater);
        local.SetWalkSpeed(swimWalk);
        local.SetRunSpeed(swimRun);
        local.SetStrafeSpeed(swimStrafe);
    }

    private void ExitWater()
    {
        inWater = false;
        local.SetGravityStrength(1f);
        local.SetWalkSpeed(2f);
        local.SetRunSpeed(4f);
        local.SetStrafeSpeed(2f);
    }
}

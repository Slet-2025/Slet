/*
 * Programmer:  labthe3rd (modified by you)
 * Date:        08/06/22
 * Description: Script that triggers event on collider enter instead of interact
 */

using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace labthe3rd
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ColliderToggle : UdonSharpBehaviour
    {
        [Header("True State String For Toggle")]
        public string trueStateString;
        [Header("False State String For Toggle")]
        public string falseStateString;
        [Space]
        [Header("Udon Behavior Which Has Event You Want To Trigger")]
        public UdonBehaviour targetUdonBehavior;
        [Header("Event Name")]
        public string targetEvent;
        [Header("Bool You Will Be Watching In Target Udon Script")]
        public string targetBoolName;

        // State of variable we are watching
        private bool toggleState;
        private UdonBehaviour localUdonBehavior;

        void Start()
        {
            localUdonBehavior = this.GetComponent<UdonBehaviour>();
            toggleState = (bool)targetUdonBehavior.GetProgramVariable(targetBoolName);
            UpdateInteractionText();
        }

        // Trigger when a player enters this object's collider
        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            // Fire event regardless of who enters
            Debug.Log($"Player entered: {player.displayName}");
            targetUdonBehavior.SendCustomEvent(targetEvent);
            SendCustomEventDelayedSeconds("DelayedUpdate", 0.1f);
        }

        // Trigger when a player exits this object's collider
        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            // Fire event regardless of who exits
            Debug.Log($"Player exited: {player.displayName}");
            targetUdonBehavior.SendCustomEvent(targetEvent);
            SendCustomEventDelayedSeconds("DelayedUpdate", 0.1f);
        }


        // Delay update slightly so event has a moment to run
        public void DelayedUpdate()
        {
            toggleState = (bool)targetUdonBehavior.GetProgramVariable(targetBoolName);
            UpdateInteractionText();
        }

        private void UpdateInteractionText()
        {
            if (localUdonBehavior != null)
            {
                localUdonBehavior.InteractionText = toggleState ? trueStateString : falseStateString;
            }
        }
    }
}

namespace NPC
{
    using UnityEngine;
    using NPC.Dialogue;

    public class ChainTrigger : MonoBehaviour
    {
        public NpcDialogue npc;
        private bool _triggered = false;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_triggered)
            {
                _triggered = true;
                npc.TriggerChainedSequence();
            }
        }
    }
}
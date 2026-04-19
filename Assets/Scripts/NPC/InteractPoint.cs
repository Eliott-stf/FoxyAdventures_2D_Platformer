namespace NPC
{
using UnityEngine;
using NPC.Dialogue;

public class InteractPoint : MonoBehaviour
{
    public NpcDialogue npcDialogue;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            npcDialogue.OnPlayerEnterRange();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            npcDialogue.OnPlayerExitRange();
    }
}
}
using Door;

namespace Collectibles.Keys
{
    using Collectibles;
    using NPC.Dialogue;
    using UnityEngine;

    public enum KeyType
    {
        ChangeColor,
        TriggerAnimation
    }

    public class Key : Collectible
    {
        [SerializeField] private KeyType keyType;
        [SerializeField] private SpriteRenderer doorSpriteRenderer;
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private DoorEntrance doorEntrance;
        [SerializeField] private NpcDialogue npcDialogue;
        //bool utilisé pour les dialogues
        public bool isCollected = false;

        protected override void Start()
        {
            //Start de la classe parent 
            base.Start();
            //On détruit le collider (porte ouverte) si elle a déja été ouverte dans le state 
            if (keyType == KeyType.TriggerAnimation && PlayerState.frontDoorUnlocked)
            {
                BoxCollider2D doorCollider = doorAnimator.GetComponent<BoxCollider2D>();
                if (doorCollider != null)
                    Destroy(doorCollider);

                // remet la porte en position ouverte
                doorAnimator.SetTrigger("Open");
            }
        }

        protected override void OnCollected(GameObject player)
        {
            switch (keyType)
            {
                //cas1: Ma clée qui fait changer de couleur la porte + la deverouille
                case KeyType.ChangeColor:
                    if (isCollected) return; // éviter de trigger plusieurs fois
                    isCollected = true; // pour les dialogues
                    if (doorSpriteRenderer != null)
                        doorSpriteRenderer.color = Color.black;
                    if (doorEntrance != null)
                        doorEntrance.Unlock(); 
                    if (npcDialogue != null)
                        npcDialogue.TriggerKeyReaction();

                    break;

                //cas2: Ma clée qui fait lance l'animation de la porte
                case KeyType.TriggerAnimation:
                    if (doorAnimator != null)
                        doorAnimator.SetTrigger("Open");

                    //On save l'état dans le state
                    PlayerState.frontDoorUnlocked = true; 

                    //On retire le collider
                        BoxCollider2D doorCollider = doorAnimator.GetComponent<BoxCollider2D>();
                    if (doorCollider != null)
                        Destroy(doorCollider);
                    break;
            }
            //On supprime la clée
            Destroy(gameObject);
        }
    }
}
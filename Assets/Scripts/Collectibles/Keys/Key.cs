namespace Collectibles.Keys
{
    using Collectibles;
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

        protected override void OnCollected(GameObject player)
        {
            switch (keyType)
            {
                case KeyType.ChangeColor:
                    if (doorSpriteRenderer != null)
                        doorSpriteRenderer.color = Color.black;
                    break;

                case KeyType.TriggerAnimation:
                    if (doorAnimator != null)
                        doorAnimator.SetTrigger("Open");
                        BoxCollider2D doorCollider = doorAnimator.GetComponent<BoxCollider2D>();
                    if (doorCollider != null)
                        Destroy(doorCollider);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
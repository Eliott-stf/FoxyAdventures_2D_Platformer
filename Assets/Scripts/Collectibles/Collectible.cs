namespace Collectibles
{
    using UnityEngine;
    using Utils;
    using Manager.Audio;

    public abstract class Collectible : MonoBehaviour
    {
        [Header("Rotation")] public float rotationSpeed = 1f;
        public Vector3 rotationAxis = Vector3.right; 
        
        void Update()
        {
            //On fait tourner l'item
            transform.Rotate(rotationAxis * rotationSpeed);
        }

        protected virtual void Start()
        {
            // si déjà collecté, on le détruit immédiatement
            if (PlayerState.collectedItems.Contains(GameUtils.GetId(gameObject)))
                Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // sauvegarde dans le state 
                PlayerState.collectedItems.Add(GameUtils.GetId(gameObject));
                // Lance le son de collecte
                SoundManager.Instance.PlaySound3D("Collect", transform.position);
                //lance la fonction onCollect et detruit l'objet
                OnCollected(other.gameObject);
                Destroy(gameObject);
            }
        }

        //méthode vide pour l'override
        protected abstract void OnCollected(GameObject player);
    }
}
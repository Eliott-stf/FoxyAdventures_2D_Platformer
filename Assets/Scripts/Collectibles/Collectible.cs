namespace Collectibles
{
    using UnityEngine;

    public abstract class Collectible : MonoBehaviour
    {
        [Header("Rotation")] public float rotationSpeed = 1f;
        public Vector3 rotationAxis = Vector3.right; 
        
        void Update()
        {
            transform.Rotate(rotationAxis * rotationSpeed);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                //lance la fonction onCollect et detruit l'objet
                OnCollected(other.gameObject);
                Destroy(gameObject);
            }
        }

        protected abstract void OnCollected(GameObject player);
    }
}
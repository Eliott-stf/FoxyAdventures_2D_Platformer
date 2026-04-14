namespace Enemies
{

    using Player.Health;
    using UnityEngine;

    public class EnemyDamage : MonoBehaviour
    {
        public PlayerHealth playerHealth;
        public int damage = 2;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Récupère la normale du premier point de contact
                Vector2 contactNormal = collision.contacts[0].normal;

                // Si le joueur arrive par le dessus 
                if (contactNormal.y < -0.5f)
                {
                    // Destroy l'ennemi
                    Destroy(gameObject);
                }
                else
                {
                    // Collision latérale donc le joueur perd de la vie + knockback
                    Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                    playerHealth.TakeDamage(damage, knockbackDir);
                }
            }
        }
    }
}

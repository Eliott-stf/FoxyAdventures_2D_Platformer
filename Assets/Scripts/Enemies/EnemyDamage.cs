namespace Enemies
{
    using Player.Health;
    using UnityEngine;
    using Utils;
    using Manager.Audio;

    public class EnemyDamage : MonoBehaviour
    {
        public PlayerHealth playerHealth;
        public int damage = 2;

        void Start()
        {
            // On regarde ds le state si l'ennemi a déjà été tué, si oui, on le détruit immédiatement
            if (PlayerState.killedEnemies.Contains(GameUtils.GetId(gameObject)))
                Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                // Récupère la normale du premier point de contact
                Vector2 contactNormal = collision.contacts[0].normal;

                // Si le joueur arrive par le dessus 
                if (contactNormal.y < -0.5f)
                {
                    // on sauvegarde l'ennemi comme tué dans le state
                    PlayerState.killedEnemies.Add(GameUtils.GetId(gameObject));
                    // Joue le son de coup
                    SoundManager.Instance.PlaySound2D("Bump");
                    // Destroy l'ennemi
                    Destroy(gameObject);
                }
                else
                {
                    // Collision latérale donc le joueur perd de la vie + knockback
                    SoundManager.Instance.PlaySound2D("Hurt");
                    Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                    playerHealth.TakeDamage(damage, knockbackDir);
                }
            }
        }
    }
}

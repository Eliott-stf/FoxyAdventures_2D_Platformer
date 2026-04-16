
using Unity.Cinemachine;

namespace Player.Health
{
    using DG.Tweening;
    using UnityEngine;
    using Player;

    public class PlayerHealth : MonoBehaviour
    {
        //propriétées de vies
        public int health;
        public int maxHealth = 3;
        
        //propriétées de recul
        private PlayerController _playerController;
        public float knockbackForce = 8f;
        public float knockbackVertical = 4f;
        
        public Transform respawnPoint;
        private CinemachineImpulseSource _impulseSource;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //on Init et récupère les components
            health = maxHealth;
            _playerController = GetComponent<PlayerController>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void TakeDamage(int amount, Vector2 knockbackDir)
        {
            //On set la vie perdu en fonciton du nb de damage de l'enemie
            health -= amount;

            //knockback
            _playerController.HorizontalVelocity = knockbackDir.x * knockbackForce;
            _playerController.VerticalVelocity = knockbackVertical;
            
            //secousse de camera avec plugin cinémachine
            _impulseSource.GenerateImpulse();

            //Si mort, on respawn au point de respawn
            if (health <= 0) Respawn();
        }
        private void Respawn()
        {
            //Reset vies et vélocité
            health = maxHealth;
            transform.position = respawnPoint.position;
            
            _playerController.HorizontalVelocity = 0f;
            _playerController.VerticalVelocity = 0f;
        }
    }
}
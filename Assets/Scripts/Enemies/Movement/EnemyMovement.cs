namespace Enemies.Movement
{
    using UnityEngine;

    public class EnemyMovement : MonoBehaviour
    {
        //sa vitesse de marche
        [SerializeField] private float speed = 3f;
        //Sa patrouille
        public GameObject pointA;
        public GameObject pointB;

        private Rigidbody2D _rb;
        //propriete pour set le point ou il doit se diriger 
        private Transform _currentTarget;

        void Start()
        {
            //Récup les component/valeur
            _rb = GetComponent<Rigidbody2D>();
            _currentTarget = pointB.transform;
        }

        void Update()
        {
            // On calcule la distance entre la position actuelle et la cible.
            if (Vector2.Distance(transform.position, _currentTarget.position) < 0.5f)
            {
                // Basculer la cible
                _currentTarget = (_currentTarget == pointB.transform)
                    ? pointA.transform
                    : pointB.transform;

                Flip();
            }

            // ON applique le mouvement vers la nouvelle cible
            float direction = (_currentTarget.position.x > transform.position.x) ? 1f : -1f;
            _rb.linearVelocity = new Vector2(direction * speed, _rb.linearVelocity.y);
        }

        //méthode pour retourner le perso
        private void Flip()
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        //méthode pour dessiner graphiquement le patrol sur la scene (confortable)
        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
            Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
            Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
        }
    }
}
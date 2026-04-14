using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    public GameObject pointA;
    public GameObject pointB;

    private Rigidbody2D rb;
    private Transform currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointB.transform;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            // Basculer la cible
            currentTarget = (currentTarget == pointB.transform) 
                ? pointA.transform 
                : pointB.transform;

            Flip();
        }

        // ON applique le mouvement vers la nouvelle cible
        float direction = (currentTarget.position.x > transform.position.x) ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }
    

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
namespace Animator
{
    using UnityEngine;

    public class BackgroundCharacter : MonoBehaviour
    {
        private Animator animator;
        private ParticleSystem dustParticle;
        public float runSpeed = 10f;
        public float startX = 1.3f; // Point A (entrée)
        public float stopX = 35f; // Milieu, où il stoppe
        public float exitX = 70.5f; // Point B (sortie)
        public float idleDuration = 2f;

        private enum State
        {
            Running,
            Idle,
            RunningOut
        }

        private State state = State.Running;
        private float idleTimer = 0f;

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            dustParticle = GetComponentInChildren<ParticleSystem>();
            transform.position = new Vector2(startX, transform.position.y);
        }

        void Update()
        {
            switch (state)
            {
                case State.Running:
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    animator.SetBool("isRunning", true);
                    dustParticle.Play();

                    if (transform.position.x >= stopX)
                    {
                        transform.position = new Vector2(stopX, transform.position.y);
                        state = State.Idle;
                        animator.SetBool("isRunning", false);
                        dustParticle.Stop(); 
                    }

                    break;

                case State.Idle:
                    idleTimer += Time.deltaTime;
                    if (idleTimer >= idleDuration)
                    {
                        idleTimer = 0f;
                        state = State.RunningOut;
                        animator.SetBool("isRunning", true);
                    }

                    break;

                case State.RunningOut:
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    dustParticle.Play();

                    if (transform.position.x >= exitX)
                    {
                        gameObject.SetActive(false);
                    }

                    break;
            }
        }
    }
}
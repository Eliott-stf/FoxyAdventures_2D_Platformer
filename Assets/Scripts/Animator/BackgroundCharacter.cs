using Player;

namespace Animator
{
    using UnityEngine;
    using Menu; // Requis pour communiquer avec le menu

    public class BackgroundCharacter : MonoBehaviour
    {
        private UnityEngine.Animator animator;
        private ParticleSystem dustParticle;

        [Header("Mouvement")]
        public float runSpeed = 10f;
        public float startX = 1.3f;
        public float stopX = 35f;
        public float exitX = 70.5f;

        [Header("Références")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private MainMenuButton mainMenu;

        [Header("HUD")]
        public GameObject tuto;
        public GameObject lifeDisplay;
        public GameObject achivements;
        
        private enum State
        {
            RunningIn,
            Waiting,
            RunningOut,
            Finished
        }

        private State state = State.RunningIn;

        void Start()
        {
            animator = GetComponentInChildren<UnityEngine.Animator>();
            dustParticle = GetComponentInChildren<ParticleSystem>();
            transform.position = new Vector2(startX, transform.position.y);

            if (playerController != null)
            {
                playerController.enabled = false;
            }
        }

        void Update()
        {
            switch (state)
            {
                case State.RunningIn:
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    animator.SetBool("isRunning", true);
                    if (!dustParticle.isPlaying) dustParticle.Play();

                    if (transform.position.x >= stopX)
                    {
                        transform.position = new Vector2(stopX, transform.position.y);
                        state = State.Waiting;
                        animator.SetBool("isRunning", false);
                        dustParticle.Stop();

                        // Déclenche l'apparition du menu
                        if (mainMenu != null)
                        {
                            mainMenu.ShowMenu();
                        }
                    }
                    break;

                case State.Waiting:
                    break;

                case State.RunningOut:
                    transform.Translate(Vector2.right * runSpeed * Time.deltaTime);
                    animator.SetBool("isRunning", true);
                    if (!dustParticle.isPlaying) dustParticle.Play();

                    if (transform.position.x >= exitX)
                    {
                        if (playerController != null)
                        {
                            playerController.enabled = true;
                        }
                        state = State.Finished;
                        animator.SetBool("isRunning", false);
                        dustParticle.Stop();
                        
                        tuto.SetActive(true);
                        lifeDisplay.SetActive(true);
                        achivements.SetActive(true);
                    }
                    break;
            }
        }

        // Méthode appelée par le script du menu
        public void TriggerExitAnimation()
        {
            state = State.RunningOut;
        }
    }
}
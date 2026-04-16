using Player;
using Unity.Cinemachine;

namespace Animator
{
    using UnityEngine;
    using Menu;

    public class BackgroundCharacter : MonoBehaviour
    {
        private UnityEngine.Animator animator;
        private ParticleSystem dustParticle;

        [Header("Mouvement")]
        public float runSpeedIn = 10f;   // vitesse A -> Pause
        public float runSpeedOut = 15f;  // vitesse Pause -> B
        public float startX = 1.3f;
        public float stopX = 35f;
        public float exitX = 70.5f;

        [Header("Références")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private MainMenuButton mainMenu;

        [Header("HUD")]
        public GameObject tuto;
        public GameObject lifeDisplay;

        [Header("Cinemachine")]
        public BoxCollider2D confiner;
        public CinemachineCamera virtualCamera;

        private enum State { RunningIn, Waiting, RunningOut, Finished }
        private State state = State.RunningIn;

        void Start()
        {
            animator = GetComponentInChildren<UnityEngine.Animator>();
            dustParticle = GetComponentInChildren<ParticleSystem>();
            transform.position = new Vector2(startX, transform.position.y);

            if (playerController != null)
                playerController.enabled = false;
        }

        void Update()
        {
            switch (state)
            {
                case State.RunningIn:
                    transform.Translate(Vector2.right * runSpeedIn * Time.deltaTime);
                    animator.SetBool("isRunning", true);
                    if (!dustParticle.isPlaying) dustParticle.Play();

                    if (transform.position.x >= stopX)
                    {
                        transform.position = new Vector2(stopX, transform.position.y);
                        state = State.Waiting;
                        animator.SetBool("isRunning", false);
                        dustParticle.Stop();

                        if (mainMenu != null)
                            mainMenu.ShowMenu();
                    }
                    break;

                case State.Waiting:
                    break;

                case State.RunningOut:
                    transform.Translate(Vector2.right * runSpeedOut * Time.deltaTime);
                    animator.SetBool("isRunning", true);
                    if (!dustParticle.isPlaying) dustParticle.Play();

                    if (transform.position.x >= exitX)
                    {
                        if (playerController != null)
                            playerController.enabled = true;

                        state = State.Finished;
                        animator.SetBool("isRunning", false);
                        dustParticle.Stop();

                        tuto.GetComponent<HUDFade>().Show();
                        lifeDisplay.GetComponent<HUDFade>().Show();

                        confiner.offset = new Vector2(39.963f, 14.736f);
                        confiner.size = new Vector2(130.181f, 35.425f);
                        virtualCamera.GetComponent<CinemachineConfiner2D>().InvalidateBoundingShapeCache();
                    }
                    break;
            }
        }

        public void TriggerExitAnimation()
        {
            state = State.RunningOut;
        }
    }
}
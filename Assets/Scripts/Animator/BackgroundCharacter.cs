using Player;
using Unity.Cinemachine;

namespace Animator
{
    using UnityEngine;
    using Menu;

    public class BackgroundCharacter : MonoBehaviour
    {
        private Animator _animator;
        private ParticleSystem _dustParticle;

        [Header("Mouvement")]
        // vitesse A -> Pause
        public float runSpeedIn = 10f;  
        // vitesse Pause -> B
        public float runSpeedOut = 15f;  
        // Coord A, Pause, B 
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
        
        [Header("Bounds")]
        public BoxCollider2D bounds;
        
        //propriètes pour stocker les component 
        private HUDFade _tutoFade;
        private HUDFade _lifeDisplayFade;
        private CinemachineConfiner2D _cameraConfiner;
        
        //enum définit la liste des statut de l'anim global
        private enum State { RunningIn, Waiting, RunningOut, Finished }
        //L'etat unitial du perso 
        private State _state = State.RunningIn;

        void Start()
        {
            //on récupère tout nos composant 
            _animator = GetComponentInChildren<Animator>();
            _dustParticle = GetComponentInChildren<ParticleSystem>();
            _tutoFade = tuto.GetComponent<HUDFade>();
            _lifeDisplayFade = lifeDisplay.GetComponent<HUDFade>();
            _cameraConfiner = virtualCamera.GetComponent<CinemachineConfiner2D>();
            
            //On place le joueur au point A 
            transform.position = new Vector2(startX, transform.position.y);

            //On désac les controls
            if (playerController != null)
                playerController.enabled = false;
        }

        void Update()
        {
            switch (_state)
            {
                //Debut de l'anim
                case State.RunningIn:
                    //On lui donne sa vitesse 
                    transform.Translate(Vector2.right * (runSpeedIn * Time.deltaTime));
                    //On set son animation pour courir
                    _animator.SetBool("isRunning", true);
                    //On active la particule 
                    if (!_dustParticle.isPlaying) _dustParticle.Play();

                    //On vérifie qu'il est bien arrivée au point Pause
                    if (transform.position.x >= stopX)
                    {
                        //On fixe sa position
                        transform.position = new Vector2(stopX, transform.position.y);
                        //On passe au state suivant
                        _state = State.Waiting;
                        //On reset son animation de course et la particule
                        _animator.SetBool("isRunning", false);
                        _dustParticle.Stop();
                        //On affiche le Menu du jeu
                        mainMenu?.ShowMenu();
                    }
                    break;
                //Pdt la pause, on fait rien
                case State.Waiting:
                    break;
                //La fin
                case State.RunningOut:
                    //On lui set sa vitesse,anim,particule..
                    transform.Translate(Vector2.right * (runSpeedOut * Time.deltaTime));
                    _animator.SetBool("isRunning", true);
                    if (!_dustParticle.isPlaying) _dustParticle.Play();

                    //On vérifie qu'il est bien arrivée au point B
                    if (transform.position.x >= exitX)
                    {
                        //On redonne les controls au joueur
                        if (playerController is not null)
                            playerController.enabled = true;

                        //On change de state, l'anim et la particule...
                        _state = State.Finished;
                        _animator.SetBool("isRunning", false);
                        _dustParticle.Stop();

                        //On affiche le HUD avec la méthode Show() de HUDFade pour un effet fondu
                        _tutoFade.Show();
                        _lifeDisplayFade.Show();

                        //Pour pas retourner au Menu, On bloque la camera ET on active le wall invisible 
                        confiner.offset = new Vector2(39.963f, 14.736f);
                        confiner.size = new Vector2(130.181f, 35.425f);
                        _cameraConfiner.InvalidateBoundingShapeCache();
                        bounds.isTrigger = false;
                    }
                    break;
            }
        }
        
        public void TriggerExitAnimation()
        {
            _state = State.RunningOut;
        }
    }
}
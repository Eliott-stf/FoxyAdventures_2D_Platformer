namespace NPC.Dialogue
{ 
    using System.Collections;
    using UnityEngine;
    using Manager;
    using Collectibles.Keys;
    using Animator;
    using Player;

    public class NpcDialogue : MonoBehaviour
    {
        #region Variables
        //Les dialogues de la scene de jour 
        [Header("Dialogues")]
        public DialogueData dialogueWithKey;
        public DialogueData dialogueWithoutKey;
        public DialogueData dialogueOnKeyCollect;

        //Dialogue de la scene de nuit 
        public DialogueData dialogueChained;
        public DialogueData dialogueAfterRescue;
        public DialogueData dialogueRecompense;

        //Les références
        [Header("Références")]
        public DialogueBubble bubble;
        public Key key;
        public GameObject buttonE;
        [SerializeField] private SleepZzz exclamation;
        [SerializeField] private PlayerController playerController;

        //Mouvement et animation de la scene de Jour
        [Header("Mouvement")]
        public float moveSpeed = 3f;
        public Transform targetPoint;
        //Spawn point du png aprés sauvetage scene de nuit
        public Transform rescueSpawnPoint;

        // Animator des persos
        [Header("Animator")]
        public UnityEngine.Animator npcAnimator;
        public UnityEngine.Animator playerAnimator;
        
        // Variable de control du durée du la transition pdt le sauvetage
        [Header("Rescue")]
        public float fadeDuration = 0.5f;
        public bool startsChained = false;

        private bool _playerInRange;
        private bool isDialogueRunning;
        private bool _isChained = true;
        private bool _rescueDone = false;
        
        private Rigidbody2D _playerRb;
        private SpriteRenderer _playerSr;
        private SpriteRenderer _sr;
        #endregion

        #region Unity Lifecycle
        void Awake()
        {
            // initialisation des références et variables
            _sr = GetComponent<SpriteRenderer>();
            if (playerController != null)
            {
                _playerRb = playerController.GetComponent<Rigidbody2D>();
                _playerSr = playerController.GetComponent<SpriteRenderer>();
            }
            _isChained = startsChained;
        }

        void Start()
        {
            if (startsChained)
                npcAnimator.SetBool("isWallGrab", true);
        }

       void Update()
        {
            // NPC de jour : dialogue selon si la clé est collectée ou non 
            if (_playerInRange && !isDialogueRunning && !_isChained && !_rescueDone && InputManager.InteractWasPressed)
            {
                DialogueData data = (key != null && key.isCollected) ? dialogueWithKey : dialogueWithoutKey;
                StartCoroutine(PlayDialogueInput(data));
            }

            // NPC de nuit enchaîné : input E pour lancer le sauvetage 
            if (_playerInRange && _isChained && !isDialogueRunning && !_rescueDone && InputManager.InteractWasPressed)
            {
                StartCoroutine(RescueSequence());
            }

            // NPC de nuit libéré : dialogue récompense après sauvetage 
            if (_playerInRange && !isDialogueRunning && _rescueDone && !_isChained && InputManager.InteractWasPressed)
            {
                StartCoroutine(PlayDialogueInput(dialogueRecompense));
            }
        }
        #endregion

        #region Triggers & Events

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                OnPlayerEnterRange();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                OnPlayerExitRange();
        }

        //Méthode appelée spécialement par l'Interact point
        public void OnPlayerEnterRange()
        {
            _playerInRange = true;
            if (!isDialogueRunning)
                buttonE.SetActive(true);
        }

        public void OnPlayerExitRange()
        {
            _playerInRange = false;
            buttonE.SetActive(false);
        }

        public void TriggerChainedSequence()
        {
            if (isDialogueRunning) return;
            StartCoroutine(ChainedSequence());
        }

        //méthode appelée par la clé quand elle est collectée pour lancer l'anim du npc
        public void TriggerKeyReaction()
        {
            if (isDialogueRunning) return;
            StartCoroutine(KeyReactionSequence());
        }
        #endregion

        #region Sequences

        // -------------------------------------------------------
        // Séquence d'enchainement du pnj
        // -------------------------------------------------------
        IEnumerator ChainedSequence()
        {
            isDialogueRunning = true;

            // On lock le player
            yield return StartCoroutine(LockPlayerRoutine());
            
            // joue l'animation d'exclamation
            exclamation.Play();
            yield return new WaitForSeconds(1.5f);
            exclamation.Stop();

            //flip le joueur vers le pnj en dur
            _playerSr.flipX = !_playerSr.flipX; 

            //animation
            playerAnimator.SetBool("isLookUp", true);

            // On lance la coroutine du dialogue automatique
            yield return StartCoroutine(PlayDialogueAuto(dialogueChained));

            // On redonne les controls au player
            playerController.enabled = true;

            //anim + flip retour à l'état initial
            playerAnimator.SetBool("isLookUp", false);
            _playerSr.flipX = !_playerSr.flipX;

            isDialogueRunning = false;
        }

        // -------------------------------------------------------
        // Séquence de sauvetage
        // -------------------------------------------------------

        IEnumerator RescueSequence()
        {
            isDialogueRunning = true;
            _rescueDone = true;
            buttonE.SetActive(false);

            // bloque le joueur
            yield return StartCoroutine(LockPlayerRoutine());

            // fondu au noir
            yield return StartCoroutine(TransitionManager.Instance.PlayFade(true, fadeDuration));

            // pendant le noir : on tp le pnj + animation
            _isChained = false;
            npcAnimator.SetBool("isWallGrab", false);
            transform.position = rescueSpawnPoint.position;

            // fondu de sortie
            yield return StartCoroutine(TransitionManager.Instance.PlayFade(false, fadeDuration));

            // redonne les controls
            playerController.enabled = true;

            // dialogue avec input après la libération
            yield return StartCoroutine(PlayDialogueInput(dialogueAfterRescue));

            isDialogueRunning = false;
        }


        // -------------------------------------------------------
        // Séquence de réaction à la collecte de la clé
        // -------------------------------------------------------
        IEnumerator KeyReactionSequence()
        {
            isDialogueRunning = true;

            // flip vers le joueur au début
            _sr.flipX = !_sr.flipX; 

            // on bloque le joueur 
            yield return StartCoroutine(LockPlayerRoutine());
            //on force l'anim idle
            playerAnimator.Play("Idle");

            // joue l'animation d'exclamation
            exclamation.Play();
            yield return new WaitForSeconds(1.5f);
            exclamation.Stop();

            // marche vers le point cible
            npcAnimator.SetBool("isWalking", true);
            while (Vector2.Distance(transform.position, targetPoint.position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(
                    transform.position,
                    targetPoint.position,
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }
            transform.position = targetPoint.position;
            npcAnimator.SetBool("isWalking", false);

            // regarde vers le haut pour le dialogue
            npcAnimator.SetBool("LookUp", true);

            // lance le dialogue automatique
            yield return StartCoroutine(PlayDialogueAuto(dialogueOnKeyCollect));

            // fin de séquence
            isDialogueRunning = false;
            npcAnimator.SetBool("LookUp", false);
            playerController.enabled = true;

            // flip retour à l'état initial
            _sr.flipX = !_sr.flipX;
        }
        #endregion

        #region Dialogues
        // Dialogue avec input joueur 
        IEnumerator PlayDialogueInput(DialogueData data)
        {
            isDialogueRunning = true;
            buttonE.SetActive(false);

            // boucle à travers les lignes de dialogue du DialogueData
            for (int i = 0; i < data.lines.Length; i++)
            {
                // vérifie si c'est la dernière ligne pour afficher ou non la flèche d'interaction
                bool isLastLine = i == data.lines.Length - 1;

                yield return StartCoroutine(bubble.ShowLine(data.lines[i]));
                bubble.SetArrowVisible(!isLastLine);

                yield return new WaitUntil(() => InputManager.InteractWasPressed);
            }

            //on cache la bulle
            yield return StartCoroutine(bubble.Hide());
            isDialogueRunning = false;
        }

        // Dialogue automatique avec timer 
        IEnumerator PlayDialogueAuto(DialogueData data)
        {
            foreach (string line in data.lines)
            {
                yield return StartCoroutine(bubble.ShowLine(line));
                yield return new WaitForSeconds(1.2f);
            }

            yield return StartCoroutine(bubble.Hide());
        }
        #endregion

        #region Utilities
        private IEnumerator LockPlayerRoutine()
        {
            //on att qu'il touche le sol
            yield return new WaitUntil(() => playerController._isGrounded);
            //on désac les controles du player + stop la vélocité
            playerController.enabled = false;
            _playerRb.linearVelocity = Vector2.zero;
            //anim pour aller en idle 
            playerAnimator.SetBool("isJumping", false);
        }
        #endregion
    }
}
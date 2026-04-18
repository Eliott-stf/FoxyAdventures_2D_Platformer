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
    //Les dialogues
    [Header("Dialogues")]
    public DialogueData dialogueWithKey;
    public DialogueData dialogueWithoutKey;
    public DialogueData dialogueOnKeyCollect;

    //Les références
    [Header("Références")]
    public DialogueBubble bubble;
    public Key key;
    public GameObject buttonE;
    [SerializeField] private SleepZzz exclamation;
    [SerializeField] private PlayerController playerController;

    //Mouvement et animation
    [Header("Mouvement")]
    public float moveSpeed = 3f;
    public Transform targetPoint;

    [Header("Animator")]
    public UnityEngine.Animator npcAnimator;

    private bool _playerInRange;
    private bool isDialogueRunning;
    
    private Rigidbody2D _playerRb;
    private SpriteRenderer _sr;
    
    void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
            if (playerController != null)
            {
                _playerRb = playerController.GetComponent<Rigidbody2D>();
            }
        }

    void Update()
    {
        //On lance le dialogue a l'input du joueur
        if (_playerInRange && !isDialogueRunning && InputManager.InteractWasPressed)
        {
            DialogueData data = key.isCollected ? dialogueWithKey : dialogueWithoutKey;
            StartCoroutine(PlayDialogueInput(data));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            // ne s'affiche pas si dialogue en cours
            if (!isDialogueRunning) 
                buttonE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            buttonE.SetActive(false);
        }
    }

    //méthode appelée par la clé quand elle est collectée pour lancer l'anim du npc
    public void TriggerKeyReaction()
    {
        if (isDialogueRunning) return;
        StartCoroutine(KeyReactionSequence());
    }

    IEnumerator KeyReactionSequence()
{
    isDialogueRunning = true;

    // flip vers le joueur au début
    _sr.flipX = !_sr.flipX;

    // attend que le joueur touche le sol
    yield return new WaitUntil(() => playerController._isGrounded);

    // bloque les controls et stoppe la vélocité
    playerController.enabled = false;
    _playerRb.linearVelocity = Vector2.zero;

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
}
}

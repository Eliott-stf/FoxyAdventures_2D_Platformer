namespace Door
{
    using UnityEngine;
    using Manager;
    using Manager.Audio;

    public class DoorEntrance : MonoBehaviour
    {
        public string sceneName = "Cave";
        private bool _playerInRange = false;
        private bool isUnlocked = false;
        public GameObject interactPrompt;
        [SerializeField] private SpriteRenderer doorSpriteRenderer;

        void Update()
        {
            //Si la porte est deverouillée, on load la scene
            if (_playerInRange && isUnlocked && InputManager.InteractWasPressed)
            {
                TransitionManager.Instance.LoadScene(sceneName, "fade");
                MusicManager.Instance.PlayMusic("Cave");
                MusicManager.Instance.PlayAmbient(PlayerState.numberOfJumpsAllowed >= 2 ? "AmbientNuit" : "AmbientMenu");
            }
        }

        void Start()
        {
            // Vérifie si la porte de la cave a été déverrouillée (depuis scene nuit)
            if (PlayerState.caveDoorsUnlocked)
            {
                Unlock();
                // restaure la couleur noire
                if (doorSpriteRenderer != null)
                    doorSpriteRenderer.color = Color.black;
            }
            
            // Vérifie si la porte d'entrée  a été déverrouillée
            if (PlayerState.frontDoorUnlocked)
            {
                Unlock();
            }
        }

        //méthode appellée quand on ramasse la clée (key;cs)
        public void Unlock()
        {
            isUnlocked = true;
            //On save dans le state pour garder la porte ouverte peut importe la scene
            PlayerState.caveDoorsUnlocked = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                if (isUnlocked)
                    interactPrompt.SetActive(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
                interactPrompt.SetActive(false);
            }
        }
    }
}
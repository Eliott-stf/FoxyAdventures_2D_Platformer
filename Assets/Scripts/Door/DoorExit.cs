namespace Door
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Manager;

    public class DoorExit : MonoBehaviour
    {
        public string sceneName = "LevelDay";
        private bool _playerInRange = false;
        public GameObject interactPrompt;

        void Update()
        {
            //Si le joueur est sur la porte et qu'il presse la touche, on load la scene
            if (_playerInRange && InputManager.InteractWasPressed)
            {
                TransitionManager.Instance.LoadScene(sceneName, "fade");
            }
        }

        void Start()
        {
            //On vérifie si le state a une scène de sortie de porte définie (après le jump potion) et on la set
            if (PlayerState.doorExitSceneName != "")
                sceneName = PlayerState.doorExitSceneName;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _playerInRange = true;
            interactPrompt.SetActive(true);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                _playerInRange = false;
            interactPrompt.SetActive(false);

        }
    }
}
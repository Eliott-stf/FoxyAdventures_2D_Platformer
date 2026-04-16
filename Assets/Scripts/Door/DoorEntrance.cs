namespace Door
{
    using UnityEngine;
    using Manager;

    public class DoorEntrance : MonoBehaviour
    {
        public string sceneName = "Cave";
        private bool _playerInRange = false;
        private bool isUnlocked = false;
        public GameObject interactPrompt;

        void Update()
        {
            //Si la porte est deverouillée, on load la scene
            if (_playerInRange && isUnlocked && InputManager.InteractWasPressed)
            {
                TransitionManager.Instance.LoadScene(sceneName, "fade");
            }
        }

        //méthode appellée quand on ramasse la clée (key;cs)
        public void Unlock()
        {
            isUnlocked = true;
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
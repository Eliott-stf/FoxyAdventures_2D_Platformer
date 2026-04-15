using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;
public class DoorExit : MonoBehaviour
{
    public string sceneName = "LevelDay";
    private bool playerInRange = false;
    public GameObject interactPrompt;

    void Update()
    {
        if (playerInRange && InputManager.InteractWasPressed)
        {
            TransitionManager.Instance.LoadScene(sceneName, "fade");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
        interactPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
        interactPrompt.SetActive(false);
        
    }
}
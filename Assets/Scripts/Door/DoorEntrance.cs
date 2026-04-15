using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;

public class DoorEntrance : MonoBehaviour
{
    public string sceneName = "Cave";
    private bool playerInRange = false;
    private bool isUnlocked = false;
    public GameObject interactPrompt;

    void Update()
    {
        if (playerInRange && isUnlocked && InputManager.InteractWasPressed)
        {
            TransitionManager.Instance.LoadScene(sceneName, "fade");
        }
    }
    //quand on ramasse la clé
    public void Unlock()
    {
        isUnlocked = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (isUnlocked)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPrompt.SetActive(false);
        }
    }
}
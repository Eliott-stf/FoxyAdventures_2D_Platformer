using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;
public class DoorExit : MonoBehaviour
{
    public string sceneName = "LevelDay";
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && InputManager.InteractWasPressed)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
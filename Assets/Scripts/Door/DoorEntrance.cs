using UnityEngine;
using UnityEngine.SceneManagement;
using Manager;

public class DoorEntrance : MonoBehaviour
{
    public string sceneName = "Cave";
    private bool playerInRange = false;
    private bool isUnlocked = false;

    void Update()
    {
        if (playerInRange && isUnlocked && InputManager.InteractWasPressed)
        {
            SceneManager.LoadScene(sceneName);
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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
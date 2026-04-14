using TMPro;

namespace Menu
{
    using UnityEngine;

    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject tuto;
        public GameObject lifeDisplay;
        public GameObject achivements;
        
        private bool hudVisible = true;
        
        public TMP_Text noHudButtonText;
        
        // Update is called once per frame
        void Update()
        {
            if (Manager.InputManager.MenuWasPressed)
            {
                if (pauseMenu.activeSelf)
                {
                    Resume(); // déjà ouvert donc on ferme
                }
                else
                {
                    pauseMenu.SetActive(true); // fermé doncon ouvre
                    Time.timeScale = 0;
                }
            }
        }
        
        public void Resume()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        public void NoHud()
        {
            hudVisible = !hudVisible;
            
            tuto.SetActive(hudVisible);
            lifeDisplay.SetActive(hudVisible);
            achivements.SetActive(hudVisible);
            
            noHudButtonText.text = hudVisible ? "Disable HUD" : "Enable HUD";
        }

        public void MainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }
}

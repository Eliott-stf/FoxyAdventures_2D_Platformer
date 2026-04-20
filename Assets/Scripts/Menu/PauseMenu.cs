using TMPro;

namespace Menu
{
    using UnityEngine;
    using Manager.Audio;

    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        //propriété des HUD
        public GameObject tuto;
        public GameObject lifeDisplay;
        
        private bool hudVisible = true;
        
        public TMP_Text noHudButtonText;
        
        // Update is called once per frame
        void Update()
        {
            if (Manager.InputManager.MenuWasPressed)
            {
                // déjà ouvert donc on ferme
                if (pauseMenu.activeSelf)
                {
                    Resume(); 
                }
                else
                {
                    // fermé doncon ouvre
                    SoundManager.Instance.PlaySound2D("UI");
                    pauseMenu.SetActive(true); 
                    Time.timeScale = 0;
                }
            }
        }
        
        //méthode pour fermer 
        public void Resume()
        {
            SoundManager.Instance.PlaySound2D("UI");
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }

        //méthode pour enlever le HUD 
        public void NoHud()
        {
            SoundManager.Instance.PlaySound2D("UI");
            //pr le toggle
            hudVisible = !hudVisible;
            
            //on affiche ou non nos elements HUD
            tuto.SetActive(hudVisible);
            lifeDisplay.SetActive(hudVisible);
            
            //on modifie le wording
            noHudButtonText.text = hudVisible ? "Disable HUD" : "Enable HUD";
        }

        //retiré 
        public void MainMenu()
        {
            SoundManager.Instance.PlaySound2D("UI");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }
    }
}

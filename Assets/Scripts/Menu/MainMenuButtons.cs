namespace Menu
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.EventSystems;
    using TMPro;

    public class MainMenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TMP_Text tmpText;
        public Color normalColor = Color.white;
        public Color hoverColor = new Color(0.69f, 0.66f, 0.66f); 

        void Start()
        {
            tmpText = GetComponentInChildren<TMP_Text>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tmpText.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tmpText.color = normalColor;
        }

        public void PlayGame()
        {
            SceneManager.LoadScene("LevelDay");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
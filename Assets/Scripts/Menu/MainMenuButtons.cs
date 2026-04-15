namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using TMPro;
    using Manager;
    using Animator; // Requis pour communiquer avec le personnage

    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Interface")]
        [SerializeField] private CanvasGroup menuCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;

        [Header("Bouton Visuel")]
        public Color normalColor = Color.white;
        public Color hoverColor = new Color(0.69f, 0.66f, 0.66f);
        private TMP_Text tmpText;

        [Header("Références Externes")]
        [SerializeField] private BackgroundCharacter backgroundCharacter;

        void Start()
        {
            tmpText = GetComponentInChildren<TMP_Text>();
            
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = 0f;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (tmpText != null) tmpText.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tmpText != null) tmpText.color = normalColor;
        }

        // Méthode appelée par le BackgroundCharacter quand il s'arrête
        public void ShowMenu()
        {
            StartCoroutine(FadeMenu(0f, 1f));
        }

        public void PlayGame()
        {
            StartCoroutine(StartGameSequence());
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private IEnumerator StartGameSequence()
        {
            // 1. Disparition du menu
            yield return StartCoroutine(FadeMenu(1f, 0f));
            menuCanvasGroup.gameObject.SetActive(false);

            // 2. Déclenchement de la course du personnage
            if (backgroundCharacter != null)
            {
                backgroundCharacter.TriggerExitAnimation();
            }
        }

        private IEnumerator FadeMenu(float from, float to)
        {
            float elapsed = 0f;
            menuCanvasGroup.alpha = from;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                menuCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
                yield return null;
            }

            menuCanvasGroup.alpha = to;
        }
    }
}
namespace Menu
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using TMPro;
    using Animator; 

    public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Interface")]
        [SerializeField] private CanvasGroup menuCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;

        [Header("Bouton Visuel")]
        public Color normalColor = Color.white;
        public Color hoverColor = new Color(0.69f, 0.66f, 0.66f);
        private TMP_Text _tmpText;
        
        [Header("Références Externes")]
        [SerializeField] private BackgroundCharacter backgroundCharacter;

        void Start()
        {
            _tmpText = GetComponentInChildren<TMP_Text>();
            
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = 0f;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tmpText != null) _tmpText.color = hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_tmpText != null) _tmpText.color = normalColor;
        }

        // Méthode appelée par le BackgroundCharacter quand il s'arrête
        public void ShowMenu()
        {
            StartCoroutine(FadeMenu(0f, 1f));
        }

        //Méthode du bouton PLay
        public void PlayGame()
        {
            StartCoroutine(StartGameSequence());
        }

        //Méthode du bouton Quit
        public void QuitGame()
        {
            Application.Quit();
        }

        private IEnumerator StartGameSequence()
        {
            // disparition du menu
            yield return StartCoroutine(FadeMenu(1f, 0f));
            menuCanvasGroup.gameObject.SetActive(false);

            // Déclenchement de l'aanimation de course du personnage
            backgroundCharacter?.TriggerExitAnimation();
        }

        //Coroutine pour faire un effet 'Fondu' au noir en jouant sur le alpha de CanvasGroup de A a B
        private IEnumerator FadeMenu(float from, float to)
        {
            //compteur de temps
            float elapsed = 0f;
            
            //Applique la transparence
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
namespace Manager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using DG.Tweening;

    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance;

        // temps de l'animation
        [SerializeField] private float duration = 0.5f;
        //UI de l'overlay
        private RectTransform _overlay;
        //Stock le mot clée a une méthode 
        private Dictionary<string, System.Func<bool, IEnumerator>> _transitions;
        private UnityEngine.UI.Image _overlayImage;

        //on execute la méthode avant meme la 1er frame
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void SpawnTransitionManager()
        {
            //On va chercher le prefab "TM" dans le dossier Ressources pour l'instancier dynamiquement
            var prefab = Resources.Load<GameObject>("TransitionManager");
            if (prefab is null) return;
            Instantiate(prefab);
        }

        void Awake()
        {
            //S'il existe déjà, on le supp
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            //on le detruit pas si ya un changement de scene 
            DontDestroyOnLoad(gameObject);

            // Trouve l'overlay automatiquement dans la scene (Qui est instancier en mm tps que TM)
            _overlay = transform.Find("../Overlay")?.GetComponent<RectTransform>()
                      ?? GameObject.Find("Overlay")?.GetComponent<RectTransform>();

            if (_overlay is null) return;
            
            _overlayImage = _overlay.GetComponent<UnityEngine.UI.Image>();

            DontDestroyOnLoad(_overlay.transform.root.gameObject);
            _overlay.anchoredPosition = new Vector2(0, Screen.height);
            RegisterTransitions();
        }

        //Méthode pour associer les mots-clés à leurs méthodes
        void RegisterTransitions()
        {
            _transitions = new Dictionary<string, System.Func<bool, IEnumerator>>
            {
                { "curtain", (isIn) => PlayCurtain(isIn) },
                { "fade",    (isIn) => PlayFade(isIn)    },
                { "flash",   (isIn) => PlayFlash(isIn)   },
            };
        }

        //Méthode appellée par les portes pour lancer une coroutine 
        public void LoadScene(string sceneName, string transitionName = "curtain")
        {
            StartCoroutine(RunTransition(sceneName, transitionName));
        }

        IEnumerator RunTransition(string sceneName, string transitionName)
        {
            //Transi est "curtaint" par defaut si rien trouvé 
            if (!_transitions.ContainsKey(transitionName))
            {
                transitionName = "curtain";
            }
    
            //start l'animation d'entrée
            yield return StartCoroutine(_transitions[transitionName](true));
            //On lance la nouvelle scene
            yield return SceneManager.LoadSceneAsync(sceneName);
            //Start l'anim de sortie
            yield return StartCoroutine(_transitions[transitionName](false));
        }
        
        /**
         * Méthode d'aniamtions pour les transitions
         */

        public IEnumerator PlayCurtain(bool isIn)
        {
            float from = isIn ? Screen.height : 0;
            float to   = isIn ? 0 : -Screen.height;

            _overlay.anchoredPosition = new Vector2(0, from);
            yield return _overlay
                .DOAnchorPosY(to, duration)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();
        }

        public IEnumerator PlayFade(bool isIn, float customDuration = -1f)
        {
            var img = _overlay.GetComponent<UnityEngine.UI.Image>();
            float d    = customDuration > 0 ? customDuration : duration;
            float from = isIn ? 0f : 1f;
            float to   = isIn ? 1f : 0f;

            _overlay.anchoredPosition = Vector2.zero;
            img.color = new Color(0, 0, 0, from);
            yield return img.DOFade(to, d).WaitForCompletion();

            // Reset overlay hors écran après fade out
            if (!isIn)
                _overlay.anchoredPosition = new Vector2(0, Screen.height);
        }

        public IEnumerator PlayFlash(bool isIn, float customDuration = -1f)
        {
            _overlay.anchoredPosition = Vector2.zero;
            _overlayImage.color = new Color(0, 0, 0, 1f);

            yield return new WaitForSeconds(isIn ? 0f : 0.1f);

            if (!isIn)
            {
                yield return _overlayImage.DOFade(0f, customDuration > 0 ? customDuration : duration).WaitForCompletion();
                _overlay.anchoredPosition = new Vector2(0, Screen.height);
            }
        }

        public IEnumerator FadeIn(float customDuration = -1f)
        {
            yield return StartCoroutine(PlayFade(true, customDuration));
        }

        public IEnumerator FadeOut(float customDuration = -1f)
        {
            yield return StartCoroutine(PlayFade(false, customDuration));
        }
    }
}
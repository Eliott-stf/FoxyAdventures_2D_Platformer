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

        [SerializeField] private float duration = 0.5f;
        private RectTransform overlay;
        private Dictionary<string, System.Func<bool, IEnumerator>> transitions;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void SpawnTransitionManager()
        {
            var prefab = Resources.Load<GameObject>("TransitionManager");
            if (prefab == null)
            {
                Debug.LogError("Prefab 'TransitionManager' introuvable dans Resources !");
                return;
            }
            Instantiate(prefab);
        }

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Trouve l'overlay automatiquement
            overlay = transform.Find("../Overlay")?.GetComponent<RectTransform>()
                      ?? GameObject.Find("Overlay")?.GetComponent<RectTransform>();

            if (overlay == null)
            {
                Debug.LogError("Overlay introuvable ! Vérifie que ton Image noire s'appelle 'Overlay'.");
                return;
            }

            DontDestroyOnLoad(overlay.transform.root.gameObject);
            overlay.anchoredPosition = new Vector2(0, Screen.height);
            RegisterTransitions();
        }

        void RegisterTransitions()
        {
            transitions = new Dictionary<string, System.Func<bool, IEnumerator>>
            {
                { "curtain", (isIn) => PlayCurtain(isIn) },
                { "fade",    (isIn) => PlayFade(isIn)    },
                { "flash",   (isIn) => PlayFlash(isIn)   },
            };
        }

        public void LoadScene(string sceneName, string transitionName = "curtain")
        {
            StartCoroutine(RunTransition(sceneName, transitionName));
        }

        IEnumerator RunTransition(string sceneName, string transitionName)
        {
            if (!transitions.ContainsKey(transitionName))
            {
                Debug.LogWarning($"Transition '{transitionName}' introuvable, fallback sur curtain.");
                transitionName = "curtain";
            }

            yield return StartCoroutine(transitions[transitionName](true));
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return StartCoroutine(transitions[transitionName](false));
        }

        IEnumerator PlayCurtain(bool isIn)
        {
            float from = isIn ? Screen.height : 0;
            float to   = isIn ? 0 : -Screen.height;

            overlay.anchoredPosition = new Vector2(0, from);
            yield return overlay
                .DOAnchorPosY(to, duration)
                .SetEase(Ease.InOutQuad)
                .WaitForCompletion();
        }

        IEnumerator PlayFade(bool isIn)
        {
            var img = overlay.GetComponent<UnityEngine.UI.Image>();
            float from = isIn ? 0f : 1f;
            float to   = isIn ? 1f : 0f;

            overlay.anchoredPosition = Vector2.zero;
            img.color = new Color(0, 0, 0, from);
            yield return img
                .DOFade(to, duration)
                .WaitForCompletion();
        }

        IEnumerator PlayFlash(bool isIn)
        {
            var img = overlay.GetComponent<UnityEngine.UI.Image>();
            overlay.anchoredPosition = Vector2.zero;
            img.color = new Color(0, 0, 0, 1f);

            yield return new WaitForSeconds(isIn ? 0f : 0.1f);

            if (!isIn)
            {
                yield return img.DOFade(0f, duration).WaitForCompletion();
                overlay.anchoredPosition = new Vector2(0, Screen.height);
            }
        }
    }
}
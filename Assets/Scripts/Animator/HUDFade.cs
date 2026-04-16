namespace Animator
{
    using DG.Tweening;
    using UnityEngine;

    public class HUDFade : MonoBehaviour
    {
        public float fadeDuration = 1f;

        void Start()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
        }
        public void Show()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            cg.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
        }

        public void Hide()
        {
            var cg = GetComponent<CanvasGroup>();
            cg.DOFade(0f, fadeDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
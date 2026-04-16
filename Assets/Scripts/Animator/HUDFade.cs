namespace Animator
{
    using DG.Tweening;
    using UnityEngine;

    public class HUDFade : MonoBehaviour
    {
        public float fadeDuration = 1f;
        private CanvasGroup _canvasGroup;

        void Awake()
        {
            //on set son oppacité a 0 (invisible)
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
        }
        
        public void Show()
        {
            //Lance l'anim de 0 -> 1
            _canvasGroup.alpha = 0f;
            _canvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InOutSine);
        }

        public void Hide()
        {
            //Lance l'anim de 1 -> 0
            _canvasGroup.DOFade(0f, fadeDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
namespace Collectibles.Coin
{
    using UnityEngine;
    using DG.Tweening;

    public class Coin : Collectible
    {
        [Header("End Sequence")]
        [SerializeField] private CanvasGroup endCanvasGroup;
        [SerializeField] private float fadeDuration = 2f;

        protected override void OnCollected(GameObject player)
        {
            if (endCanvasGroup != null)
            {
                endCanvasGroup.gameObject.SetActive(true);
                endCanvasGroup.alpha = 0f;
                endCanvasGroup.DOFade(1f, fadeDuration).SetEase(Ease.InSine);
            }
        }
    }
}
namespace Animator
{
    using UnityEngine;
    using DG.Tweening;

    public class ButtonPopUp : MonoBehaviour
    {
        public Vector3 targetScale = new Vector3(0.058f, 0.045f, 0.045f); // ta scale de base

        void OnEnable()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(targetScale, 0.4f).SetEase(Ease.OutBack);
        }
    }
}
namespace Animator
{
    using UnityEngine;
    using DG.Tweening;

    public class ButtonPopUp : MonoBehaviour
    {
        private Vector3 _originalScale;

        void Awake()
        {
            //on set la valeur de base
            _originalScale = transform.localScale;
        }

        void OnEnable()
        {
            //anim 
            transform.localScale = Vector3.zero;
            transform.DOScale(_originalScale, 0.4f).SetEase(Ease.OutBack);
        }
    }
}
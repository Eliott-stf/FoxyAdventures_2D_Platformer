namespace Animator
{
    using UnityEngine;
    using DG.Tweening;

    public class TitleAnimator : MonoBehaviour
    {
        [Header("Scale Settings")]
        [SerializeField] private float scaleMultiplier = 1.15f;
        [SerializeField] private float scaleDuration = 2.0f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationAngle = 5f;
        [SerializeField] private float rotationDuration = 1.5f;

        private Vector3 _initialScale;
        private Vector3 _initialRotation;

        private void Start()
        {
            //on set les valeur d'origine
            _initialScale = transform.localScale;
            _initialRotation = transform.localEulerAngles;

            //Anime d'agrandissement
            transform.DOScale(_initialScale * scaleMultiplier, scaleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            //set de l'angle
            transform.localEulerAngles = new Vector3(_initialRotation.x, _initialRotation.y, _initialRotation.z - rotationAngle);

            //Anim de rotation
            transform.DOLocalRotate(new Vector3(_initialRotation.x, _initialRotation.y, _initialRotation.z + rotationAngle), rotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
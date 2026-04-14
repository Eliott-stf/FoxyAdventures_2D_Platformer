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

        private Vector3 initialScale;
        private Vector3 initialRotation;

        private void Start()
        {
            initialScale = transform.localScale;
            initialRotation = transform.localEulerAngles;

            transform.DOScale(initialScale * scaleMultiplier, scaleDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            transform.localEulerAngles = new Vector3(initialRotation.x, initialRotation.y, initialRotation.z - rotationAngle);

            transform.DOLocalRotate(new Vector3(initialRotation.x, initialRotation.y, initialRotation.z + rotationAngle), rotationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }
    }
}
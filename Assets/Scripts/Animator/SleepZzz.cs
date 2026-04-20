namespace Animator
{
    using UnityEngine;
    using TMPro;
    using DG.Tweening;
    using System.Collections;
    using Manager.Audio;

    public class SleepZzz : MonoBehaviour
    {
        [Header("Références")] public TMP_Text z1, z2, z3;

        [Header("Paramètres de Temps")] public float delay = 0.4f;
        public float animationDuration = 2.0f;
        public float fadeInDuration = 0.5f;
        public float fadeOutDuration = 0.5f;

        [Header("Paramètres d'Échelle")] public float startScale = 0.3f;
        public float endScale = 1.0f;

        [Header("Paramètres de Trajectoire")]
        public AnimationCurve curveSpeed = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        public void Play()
        {
            gameObject.SetActive(true);
            StartCoroutine(AnimateZs());
            
        }

        public void Stop()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        IEnumerator AnimateZs()
        {
            while (true)
            {
                yield return AnimateZ(z1);
                yield return new WaitForSeconds(delay);
                yield return AnimateZ(z2);
                yield return new WaitForSeconds(delay);
                yield return AnimateZ(z3);
                yield return new WaitForSeconds(0.8f);
            }
        }

        IEnumerator AnimateZ(TMP_Text z)
        {
            Vector3 startPos = z.transform.localPosition;
            z.transform.localScale = Vector3.one * startScale;

            z.DOFade(1f, fadeInDuration);
            z.transform.DOScale(endScale, fadeInDuration).SetEase(Ease.OutBack);

            Vector3[] path = new Vector3[]
            {
                startPos + new Vector3(20f, 15f, 0f),
                startPos + new Vector3(30f, 40f, 0f)
            };

            z.transform.DOLocalPath(path, animationDuration, PathType.CatmullRom).SetEase(curveSpeed);

            float waitTime = Mathf.Max(0f, animationDuration - fadeOutDuration);
            yield return new WaitForSeconds(waitTime);

            yield return z.DOFade(0f, fadeOutDuration).WaitForCompletion();

            z.transform.localPosition = startPos;
            z.transform.localScale = Vector3.one * startScale;
        }
    }
}
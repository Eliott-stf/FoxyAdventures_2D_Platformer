namespace Animator
{

    using System.Collections;
    using UnityEngine;
    using UnityEngine.Rendering.Universal;

    public class SunFlare : MonoBehaviour
    {
        private Light2D _light;
        private float _defaultIntensity;

        [SerializeField] private float peakIntensity = 5f;
        [SerializeField] private float duration = 1f;

        void Awake()
        {
            _light = GetComponent<Light2D>();
            _defaultIntensity = _light.intensity;
        }

        // appelé par le Signal Timeline
        public void TriggerFlare()
        {
            StartCoroutine(FlareSequence());
        }

        IEnumerator FlareSequence()
        {
            // monte à l'intensité max
            yield return StartCoroutine(LerpIntensity(_defaultIntensity, peakIntensity, duration / 2));
            // redescend à l'intensité de base
            yield return StartCoroutine(LerpIntensity(peakIntensity, _defaultIntensity, duration / 2));
        }

        IEnumerator LerpIntensity(float from, float to, float time)
        {
            float elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                _light.intensity = Mathf.Lerp(from, to, elapsed / time);
                yield return null;
            }
            _light.intensity = to;
        }
    }
}
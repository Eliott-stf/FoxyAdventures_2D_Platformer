namespace Animator
{
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Fire : MonoBehaviour
{
    [SerializeField] private float baseIntensity = 1.07f;
    [SerializeField] private float flickerAmount = 0.2f;
    [SerializeField] private float flickerSpeed = 8f;

    private Light2D light2D;

    void Awake() => light2D = GetComponent<Light2D>();

    void Update()
    {
        light2D.intensity = baseIntensity + 
            Mathf.PerlinNoise(Time.time * flickerSpeed, 0f) * flickerAmount;
    }
}
}
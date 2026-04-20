namespace Animator
{using UnityEngine;
using DG.Tweening;

public class Fee : MonoBehaviour
{
    //Propriétés pour l'animation 
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float duration = 1f;

    // Propriétés pour la couleur (sa modif le matériaux directement)
    [SerializeField] private Color couleur1 = Color.white; 
    [SerializeField] private Color couleur2 = Color.black; 
    [SerializeField] private string shaderColorProperty = "_BaseColor"; 

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        // Initialisation à l'état minimum
        transform.localScale = Vector3.one * minScale;
        _renderer.material.SetColor(shaderColorProperty, couleur2);

        // Interpolation de l'échelle
        transform.DOScale(maxScale, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // Interpolation de la couleur du matériau
        _renderer.material.DOColor(couleur1, shaderColorProperty, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
}
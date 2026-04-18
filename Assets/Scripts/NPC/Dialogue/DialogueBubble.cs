using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class DialogueBubble : MonoBehaviour
{
    //Références
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject arrow;

    //propriétés de timing
    [Header("Timing")]
    public float typeSpeed = 0.04f;
    public float fadeDuration = 0.2f;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        //Initialisation
        canvasGroup = GetComponent<CanvasGroup>();
        bubble.SetActive(false);
    }

    public IEnumerator ShowLine(string line)
    {
        arrow.SetActive(false);
        // Affiche la bulle
        bubble.SetActive(true);
        // Fade in
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeDuration);

        // Effet machine à écrire
        text.text = "";
        foreach (char c in line)
        {
            text.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public IEnumerator Hide()
    {
        // Fade out
        yield return canvasGroup.DOFade(0f, fadeDuration).WaitForCompletion();
        bubble.SetActive(false);
        text.text = "";
    }

// méthode pour afficher la flèche d'interaction
    public void SetArrowVisible(bool visible)
    {
        arrow.SetActive(visible);
    }
}
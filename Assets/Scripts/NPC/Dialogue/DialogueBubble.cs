namespace NPC.Dialogue
{
    using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
    using Manager.Audio;

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

    private CanvasGroup _canvasGroup;

    void Awake()
    {
        //Initialisation
        _canvasGroup = GetComponent<CanvasGroup>();
        bubble.SetActive(false);
    }

    public IEnumerator ShowLine(string line)
    {
        arrow.SetActive(false);
        // Affiche la bulle
        bubble.SetActive(true);
        // Fade in
        _canvasGroup.alpha = 0f;
        _canvasGroup.DOFade(1f, fadeDuration);

        // Lance le son de dialogue
        SoundManager.Instance.PlaySound2D("Dialogue");

        // Effet machine à écrire
        text.text = "";
        foreach (char c in line)
        {
            text.text += c; 
            yield return new WaitForSeconds(typeSpeed);
        }

        // Arrête le son à la fin de la ligne
        SoundManager.Instance.StopSound();
    }

    public IEnumerator Hide()
    {
        // Fade out
        yield return _canvasGroup.DOFade(0f, fadeDuration).WaitForCompletion();
        bubble.SetActive(false);
        text.text = "";
    }

    // méthode pour afficher la flèche d'interaction
    public void SetArrowVisible(bool visible)
    {
        arrow.SetActive(visible);
    }
}
}

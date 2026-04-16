namespace Animator
{
    using UnityEngine;

    public class Wind : MonoBehaviour
    {
        // amplitude (+2 / -2)
        public float swayAmount = 2f;
        // vitesse du balancement
        public float swaySpeed = 1f; 
        // décalage pour que chaque arbre soit désynchronisé
        public float randomOffset = 0f; 

        // Stocke la rotation initiale de l'objet
        private Quaternion _startRotation;

        void Start()
        {
            // on set l'orientation d'origine
            _startRotation = transform.rotation;
            // offset aléatoire au démarrage
            randomOffset = Random.Range(0f, 2f * Mathf.PI); 
        }

        void Update()
        {
            //on set l'angle
            float angle = Mathf.Sin(Time.time * swaySpeed + randomOffset) * swayAmount;
            //on applique l'angle
            transform.rotation = _startRotation * Quaternion.Euler(0f, 0f, angle);
        }
    }
}
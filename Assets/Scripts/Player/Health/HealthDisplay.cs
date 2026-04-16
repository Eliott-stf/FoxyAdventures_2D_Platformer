namespace Player.Health
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthDisplay : MonoBehaviour
    {
        //propriétées de vies
        public int health;
        public int maxHealth;
        public PlayerHealth playerHealth;

        //sprites
        public Sprite emptyHearth;
        public Sprite fullHearth;
        public Image[] hearts;

        // Update is called once per frame
        void Update()
        {
            //init avec la vie du player 
            health = playerHealth.health;
            maxHealth = playerHealth.maxHealth;
            
            for (int i = 0; i < hearts.Length; i++)
            {
                // Active ou désactive l'objet selon le maxHealth
                hearts[i].gameObject.SetActive(i < maxHealth);
                hearts[i].sprite = i < health ? fullHearth : emptyHearth;
            }
        }
    }
}
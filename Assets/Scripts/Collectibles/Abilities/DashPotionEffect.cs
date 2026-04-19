using Manager;

namespace Collectibles.Abilities
{
    using Collectibles;
    using UnityEngine;
    using Player;
    using TMPro;

    public class DashPotionEffect : Collectible
    {
        public TMP_Text dashText;
        protected override void OnCollected(GameObject player)
        {
            PlayerController pc = player.GetComponentInParent<PlayerController>();
            if (pc != null)
            {
                //on lui ajoute un dash + dans le state
                pc.MoveStats.numberOfDashes = 1;
                PlayerState.numberOfDashes = 1;
                //set la couleur du text du Tuto
                dashText.color = new Color(0xE1 / 255f, 0xA9 / 255f, 0xA9 / 255f);
                //Lance l'anim du canva d'achievement "dash"
                FindFirstObjectByType<AchievementManager>().Unlock("Dash");
            }
        }
    }
}
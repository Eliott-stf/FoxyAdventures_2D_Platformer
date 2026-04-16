using Manager;

namespace Collectibles.Abilities
{
    using Player;
    using UnityEngine;
    using Collectibles;
    using TMPro;
    
    public class JumpPotionEffect : Collectible
    {
        public TMP_Text doubleJumpText;
        protected override void OnCollected(GameObject player)
        {
            PlayerController pc = player.GetComponentInParent<PlayerController>();
            if (pc != null)
            {
                //On lui ajoute un jump
                pc.MoveStats.numberOfJumpsAllowed = 2;
                //set la couleur du text du Tuto
                doubleJumpText.color = new Color(0xE1 / 255f, 0xA9 / 255f, 0xA9 / 255f);
                //Lance l'anim du canva d'achievement "jump"
                FindFirstObjectByType<AchievementManager>().Unlock("Jump");
            }
        }
    }
}
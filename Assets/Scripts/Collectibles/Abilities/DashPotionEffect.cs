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
                pc.MoveStats.numberOfDashes = 1;
                dashText.color = new Color(0xE1 / 255f, 0xA9 / 255f, 0xA9 / 255f);
                FindFirstObjectByType<AchievementManager>().Unlock("Dash");
            }
        }
    }
}
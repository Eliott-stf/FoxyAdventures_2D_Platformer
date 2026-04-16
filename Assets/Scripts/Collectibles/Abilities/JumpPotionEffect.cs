using Manager;

namespace Collectibles.Abilities
{
    using Player;
    using UnityEngine;
    using Collectibles;
    using TMPro;
    
    public class JumpPotionEffect : Collectible
    {
        [Header("Références")]
        public Animator playerAnimator;
        public PlayerController playerController;
        public DoorEntrance door; // ta porte qui mène à LevelNight
        
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
                // Lance la coroutine depuis le TransitionManager
                TransitionManager.Instance.StartCoroutine(SleepSequence(pc));
            }
        }
        
        IEnumerator SleepSequence()
        {
            // 1. Bloque les controls + lance anim sleep
            playerController.enabled = false;
            playerAnimator.SetBool("isSleeping", true);

            // 2. Fondu au noir (réutilise PlayFade du TransitionManager)
            yield return StartCoroutine(
                Manager.TransitionManager.Instance.PlayFadePublic(true)
            );

            // 3. Son dodo
            if (sleepSound != null)
                audioSource.PlayOneShot(sleepSound);

            // 4. Attend 3 secondes dans le noir
            yield return new WaitForSeconds(3f);

            // 5. Set la scène sur la porte
            door.sceneName = "LevelNight";

            // 6. isSleeping false
            playerAnimator.SetBool("isSleeping", false);

            // 7. Fondu de sortie
            yield return StartCoroutine(
                Manager.TransitionManager.Instance.PlayFadePublic(false)
            );

            // 8. Redonne les controls
            playerController.enabled = true;
        }
    }
}
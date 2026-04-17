using System.Collections;
using Animator;
using Door;
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
        public DoorExit door; 
        public SleepZzz SleepZzz; 
        
        [Header("Transition")]
        public float fadeDuration = 1f;
        public float fadeDuration2 = 1f;
        
        
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
        
        IEnumerator SleepSequence(PlayerController pc)
        {
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            //1. On bloque les controls + stop la vélocité quand on touche le sol
            yield return new WaitUntil(() => pc._isGrounded);
            playerController.enabled = false;
            rb.linearVelocity = Vector2.zero;
            
            //2. On lance l'aniamtion de dodo + les particules
            playerAnimator.SetBool("isSleeping", true);
            SleepZzz.Play();

            //3. Fondu au noir (réutilise PlayFade du TransitionManager)
            yield return TransitionManager.Instance.StartCoroutine(
                TransitionManager.Instance.PlayFade(true, fadeDuration)
            );
            
            //4. Stop des particules
            SleepZzz.Stop();
            
            //5.  Attend 2 secondes dans le noir
            yield return new WaitForSeconds(2f);
            
            //6. Set la scène sur la porte
            door.sceneName = "Night";
            
            //7. Fondu de sortie
            yield return TransitionManager.Instance.StartCoroutine(
                TransitionManager.Instance.PlayFade(false, fadeDuration2)
            );

            //8. Change l'naimation 
            playerAnimator.SetBool("isSleeping", false);
            
            //9. Redonne les controls
            pc.enabled = true;
        }
    }
}
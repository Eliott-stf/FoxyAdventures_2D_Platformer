namespace UI
{
    using UnityEngine;
    using TMPro;
    using Player;

    public class HUDInitializer : MonoBehaviour
    {
        [SerializeField] private TMP_Text doubleJumpText;
        [SerializeField] private TMP_Text dashText;

        void Start()
        {
            // Vérifie si le double jump a été débloqué dans le PlayerState
            if (PlayerState.numberOfJumpsAllowed >= 2 && doubleJumpText != null)
            {
                doubleJumpText.color = new Color(0xE1 / 255f, 0xA9 / 255f, 0xA9 / 255f);
            }

            // Vérifie si le dash a été débloqué dans le PlayerState
            if (PlayerState.numberOfDashes >= 1 && dashText != null)
            {
                dashText.color = new Color(0xE1 / 255f, 0xA9 / 255f, 0xA9 / 255f);
            }
        }
    }
}

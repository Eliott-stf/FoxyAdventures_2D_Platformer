namespace Manager
{
    using UnityEngine;
    using DG.Tweening;
    using Manager.Audio;

    public class AchievementManager : MonoBehaviour
    {
        [System.Serializable]
        public class Achievement
        {
            // Identifiant textuel du succès pour la recherche.
            public string name;
            public RectTransform panel;
        }
        //tab qui contient l'ensemble des succès 
        public Achievement[] achievements;
        //temps de durée de l'anim
        public float duration = 0.5f;


        //anim pour faire glisser le canvas achevement 
        private void ShowAchievement(RectTransform panel)
        {
            //On positionne en dur en dehors du canva 
            Vector2 startPos = new Vector2(753, panel.anchoredPosition.y);
            Vector2 endPos = new Vector2(427, panel.anchoredPosition.y);

            //anim de la pos A et B smooth
            panel.anchoredPosition = startPos;
            panel.gameObject.SetActive(true);
            // Son d'entrée de l'achievement
            SoundManager.Instance.PlaySound2D("Dash");
            panel.DOAnchorPos(endPos, duration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        // Son de sortie de l'achievement
                        SoundManager.Instance.PlaySound2D("Dash");
                        panel.DOAnchorPos(startPos, duration)
                            .SetEase(Ease.InBack)
                            .OnComplete(() => panel.gameObject.SetActive(false));
                    });
                });
        }
        public void Unlock(string achievementName)
        {
            foreach (var a in achievements)
            {
                if (a.name == achievementName)
                {
                    ShowAchievement(a.panel);
                    return;
                }
            }
        }
    }
}

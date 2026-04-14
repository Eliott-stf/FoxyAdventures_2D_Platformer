namespace Manager
{
    using UnityEngine;
    using DG.Tweening;

    public class AchievementManager : MonoBehaviour
    {
        [System.Serializable]
        public class Achievement
        {
            public string name;
            public RectTransform panel;
        }

        public Achievement[] achievements;
        public float duration = 0.5f;

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

        private void ShowAchievement(RectTransform panel)
        {
            Vector2 startPos = new Vector2(753, panel.anchoredPosition.y);
            Vector2 endPos = new Vector2(427, panel.anchoredPosition.y);

            panel.anchoredPosition = startPos;
            panel.gameObject.SetActive(true);

            panel.DOAnchorPos(endPos, duration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        panel.DOAnchorPos(startPos, duration)
                            .SetEase(Ease.InBack)
                            .OnComplete(() => panel.gameObject.SetActive(false));
                    });
                });
        }
    }
}
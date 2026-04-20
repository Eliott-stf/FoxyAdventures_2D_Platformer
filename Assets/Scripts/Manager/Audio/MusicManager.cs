using System.Collections;
using UnityEngine;
using Audio;


namespace Manager.Audio
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;

        [SerializeField] private MusicLibrary musicLibrary;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource ambientSource;

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void PlayMusic(string trackName, float fadeDuration = 0.5f)
        {
            StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
        }

        public void PlayAmbient(string trackName)
        {
            ambientSource.clip = musicLibrary.GetClipFromName(trackName);
            ambientSource.Play();
        }

        IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
        {
            float percent = 0;
            while (percent < 1)
            {
                percent += Time.deltaTime / fadeDuration;
                musicSource.volume = Mathf.Lerp(1f, 0f, percent);
                yield return null;
            }

            musicSource.clip = nextTrack;
            musicSource.Play();
            percent = 0;

            while (percent < 1)
            {
                percent += Time.deltaTime / fadeDuration;
                musicSource.volume = Mathf.Lerp(0f, 1f, percent);
                yield return null;
            }
        }
    }
}

namespace Audio
{
    using UnityEngine;

    namespace Audio
    {
        [System.Serializable]
        public struct AmbientSound
        {
            public string soundName;
            public AudioClip clip;
        }

        public class AmbientLibrary : MonoBehaviour
        {
            public AmbientSound[] ambientSounds;

            public AudioClip GetClipFromName(string name)
            {
                foreach (var sound in ambientSounds)
                    if (sound.soundName == name)
                        return sound.clip;
                return null;
            }
        }
    }
}
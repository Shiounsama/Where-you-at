using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundDesign
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager Instance;

        [SerializeField] private SoundBankSO soundBank;

        public SoundBankSO SoundBank { get { return soundBank; } }

        [SerializeField] private AudioSource audioSourcePrefab;

        private AudioSource m_audioSource;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (!Instance)
                Instance = this;

            m_audioSource = GetComponent<AudioSource>();

            m_audioSource.clip = soundBank.backgroundMusic;
            m_audioSource.Play();
        }

        public void PlaySFXClip(AudioClip clip, Transform spawnTransform, float volume = 1f)
        {
            AudioSource audioSource = Instantiate(audioSourcePrefab, spawnTransform.position, Quaternion.identity);

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundDesign
{
    public class SoundMixerManager : MonoBehaviour
    {
        public static SoundMixerManager Instance;

        public AudioMixer AudioMixer { get; private set; }
        public bool isMuted { get; private set; } = false;

        private float lastMasterVolumeValue = Mathf.Log10(1) * 20f;

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (!Instance)
                Instance = this;

            AudioMixer = Resources.Load("Sound Design/MainMixer") as AudioMixer;
        }

        public void SetMasterVolume(float level)
        {
            float volumeValue = Mathf.Log10(level) * 20f;
            lastMasterVolumeValue = volumeValue;

            if (isMuted)
                return;

            AudioMixer.SetFloat("masterVolume", volumeValue);
        }

        public void SetSFXVolume(float level)
        {
            float volumeValue = Mathf.Log10(level) * 20f;
            AudioMixer.SetFloat("sfxVolume", volumeValue);
        }

        public void SetMusicVolume(float level)
        {
            float volumeValue = Mathf.Log10(level) * 20f;
            AudioMixer.SetFloat("musicVolume", volumeValue);
        }

        public void MuteMasterVolume(bool mute)
        {
            float volumeValue = 0;

            if (mute)
            {
                volumeValue = Mathf.Log10(0.0001f) * 20f;
            }
            else
            {
                volumeValue = lastMasterVolumeValue;
            }

            AudioMixer.SetFloat("masterVolume", volumeValue);
        }

        public void ToggleMasterVolume()
        {
            isMuted = !isMuted;

            MuteMasterVolume(isMuted);
        }
    }
}
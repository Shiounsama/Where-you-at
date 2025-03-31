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

        private void Awake()
        {
            DontDestroyOnLoad(this);

            if (!Instance)
                Instance = this;

            AudioMixer = Resources.Load("Sound Design/MainMixer") as AudioMixer;
        }

        public void SetMasterVolume(float level)
        {
            AudioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSFXVolume(float level)
        {
            AudioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
        }

        public void SetMusicVolume(float level)
        {
            AudioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        }
    }
}
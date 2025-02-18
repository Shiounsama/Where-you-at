using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundDesign
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer m_audioMixer;

        public void SetMasterVolume(float level)
        {
            m_audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
        }

        public void SetSFXVolume(float level)
        {
            m_audioMixer.SetFloat("sfxVolume", Mathf.Log10(level) * 20f);
        }

        public void SetMusicVolume(float level)
        {
            m_audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
        }
    }
}
using SoundDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public abstract class Building : MonoBehaviour
{
    private protected AudioClip ambientSound;
    private protected SoundBankSO _soundBank;

    private AudioSource m_audioSource;

    private Camera localPlayerCam;

    private IEnumerator Start()
    {
        Debug.Log("Start Building");

        _soundBank = SoundFXManager.Instance.SoundBank;

        m_audioSource = GetComponent<AudioSource>();
        InitializeAudioSource();

        yield return new WaitForSeconds(3f);
        
        localPlayerCam = manager.Instance.GetLocalPlayerData().GetComponentInChildren<Camera>();
        Debug.Log(localPlayerCam);
        SetAmbientSound();
    }

    private void Update()
    {
        if (localPlayerCam)
        {
            m_audioSource.volume = Mathf.InverseLerp(8, 2, localPlayerCam.orthographicSize);
        }
    }

    private void InitializeAudioSource()
    {
        m_audioSource.loop = true;
        m_audioSource.outputAudioMixerGroup = SoundMixerManager.Instance.AudioMixer.FindMatchingGroups("SFX")[0];
        m_audioSource.spatialBlend = 1f;
        m_audioSource.dopplerLevel = 0f;
        m_audioSource.rolloffMode = AudioRolloffMode.Linear;
        m_audioSource.minDistance = 59f;
        m_audioSource.maxDistance = 62f;
    }

    public virtual void SetAmbientSound()
    {
        m_audioSource.clip = ambientSound;
        m_audioSource.Play();
    }
}

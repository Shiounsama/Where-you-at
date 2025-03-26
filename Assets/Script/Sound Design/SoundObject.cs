using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoundDesign
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class SoundObject : MonoBehaviour
    {
        [SerializeField] private float soundDistance = 5f;

        [Header("Debug")]
        [SerializeField] private Color wireSphereColor = Color.red;
        [SerializeField] private int labelFontSize = 15;

        private protected AudioClip ambientSound;
        private protected SoundBankSO _soundBank;

        private AudioSource m_audioSource;
        private Camera _localPlayerCam;
        private PlayerData _playerData;
        private SeekerAudio _seekerAudio;

        private IEnumerator Start()
        {
            //Debug.Log("Start Building");

            _soundBank = SoundFXManager.Instance.SoundBank;

            m_audioSource = GetComponent<AudioSource>();
            InitializeAudioSource();

            yield return new WaitForSeconds(.1f);

            _playerData = manager.Instance.GetLocalPlayerData();
            _localPlayerCam = _playerData.GetComponentInChildren<Camera>();
            _seekerAudio = _localPlayerCam.GetComponent<SeekerAudio>();
            //Debug.Log(localPlayerCam);
            SetAmbientSound();
        }

        private void Update()
        {
            if (_localPlayerCam)
            {
                if (_playerData.role == Role.Lost)
                    return;

                float projectedCameraDistance = Vector3.Distance(_seekerAudio.projectedCameraPos, transform.position);
                Debug.Log($"Projected camera distance: {projectedCameraDistance}");
                m_audioSource.volume = Mathf.InverseLerp(soundDistance, 0, projectedCameraDistance) * Mathf.InverseLerp(8, 2, _localPlayerCam.orthographicSize);
            }
        }

        private void InitializeAudioSource()
        {
            m_audioSource.loop = true;
            m_audioSource.outputAudioMixerGroup = SoundMixerManager.Instance.AudioMixer.FindMatchingGroups("SFX")[0];
        }

        public virtual void SetAmbientSound()
        {
            m_audioSource.clip = ambientSound;
            m_audioSource.Play();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = wireSphereColor;
            Gizmos.DrawWireSphere(transform.position, soundDistance);
            
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = labelFontSize;
            Handles.Label(transform.position + Vector3.up * 3, this.GetType().Name, gUIStyle);
        }
#endif
    }
}
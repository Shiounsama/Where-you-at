using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundDesign
{
    [CreateAssetMenu(fileName = "SoundBank_", menuName = "Sound Design/Create a sound bank", order = 0)]
    public class SoundBankSO : ScriptableObject
    {
        [Header("SFXs")]
        public AudioClip gameEndingCelebrationClip;
        public AudioClip roleWheelTickClip;

        [Header("UI")]
        public AudioClip buttonClickClip;
        public AudioClip buttonHoverClip;
        public AudioClip cancelButtonClickClip;

        public AudioClip textTypingClip;

        public AudioClip pnjPingClip;

        [Header("Music")]
        public AudioClip backgroundMusicGame;
        public AudioClip backgroundMusicMenu;

        [Header("Interest points")]
        public AudioClip ferrisWheelMusic;
    }
}
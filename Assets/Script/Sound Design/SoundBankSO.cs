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
        public AudioClip buttonClick_ConfirmJoin_Clip;
        public AudioClip buttonClick_SendMessage_Clip;
        public AudioClip buttonClick_Guess_Clip;
        public AudioClip buttonClick_Ready_Clip;
        public AudioClip buttonClick_Unready_Clip;
        public AudioClip buttonClick_AskQuestion_Clip;

        public AudioClip textTypingClip;

        public AudioClip pnjPingClip;

        [Header("Music")]
        public AudioClip backgroundMusicGame;
        public AudioClip backgroundMusicMenu;

        [Header("Interest points")]
        public AudioClip ferrisWheelMusic;
    }
}
using SoundDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClick_Unready_Clip;
    }

    public void ToggleAudioClip()
    {
        buttonClickClip = buttonClickClip == SoundBank.buttonClick_Ready_Clip ? SoundBank.buttonClick_Unready_Clip : SoundBank.buttonClick_Ready_Clip;
    }
}

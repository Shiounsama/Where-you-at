using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskQuestionButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClick_AskQuestion_Clip;
    }
}

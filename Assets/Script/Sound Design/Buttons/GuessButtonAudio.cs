using SoundDesign;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClick_Guess_Clip;
    }
}

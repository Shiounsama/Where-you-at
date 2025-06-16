using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClick_SendMessage_Clip;
    }
}

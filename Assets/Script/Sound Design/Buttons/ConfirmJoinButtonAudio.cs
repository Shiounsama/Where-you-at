using UnityEngine;

public class ConfirmJoinButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClick_ConfirmJoin_Clip;
    }
}

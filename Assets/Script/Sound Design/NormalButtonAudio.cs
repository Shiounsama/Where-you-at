using UnityEngine;

public class NormalButtonAudio : ButtonAudio
{
    public override void Start()
    {
        base.Start();

        buttonClickClip = SoundBank.buttonClickClip;
    }
}

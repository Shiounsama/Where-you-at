using System.Collections;
using System.Collections.Generic;
using SoundDesign;
using UnityEngine;

public class FerrisWheel : SoundObject
{
    public override void SetAmbientSound()
    {
        ambientSound = _soundBank.ferrisWheelMusic;

        base.SetAmbientSound();
    }
}

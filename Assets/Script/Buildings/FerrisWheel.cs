using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheel : Building
{
    public override void SetAmbientSound()
    {
        ambientSound = _soundBank.ferrisWheelMusic;

        base.SetAmbientSound();
    }
}

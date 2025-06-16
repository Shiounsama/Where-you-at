using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostView : View
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);

        TchatPanel?.SetActive(true);
    }
}

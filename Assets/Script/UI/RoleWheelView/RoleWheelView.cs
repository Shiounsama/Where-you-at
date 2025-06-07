using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleWheelView : View
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);

        FindObjectOfType<RoleWheel>(true).enabled = true;
    }
}

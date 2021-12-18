using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli : EnemyCar
{
    enum State
    {
        CatchUp,
        Stay,
        Leave,
    }

    protected override void ChangeSpeed()
    {
        base.ChangeSpeed();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpCar : NomalCar
{
    protected override void SetTag()
    {
        //タグ設定
        KMath.SetTag(gameObject, "Jumper");
    }
}

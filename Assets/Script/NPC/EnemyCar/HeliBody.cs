using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliBody : EnemyCarBody
{
    [SerializeField]
    Rigidbody selfRigidbody;

    public override void Init(EnemyCar car)
    {
        base.Init(car);
        targetLane = Random.Range(0, StageDatabase.RoadData.Lane);
    }

    public override void DeadPush(Vector3 move, Vector3 rotate, Rigidbody rb = null)
    {
        base.DeadPush(move, rotate, selfRigidbody);
    }

    protected override void ChangeTargetLane()
    {
        //doNothing
    }
}

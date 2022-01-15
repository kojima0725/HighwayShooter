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

    public override Type CarType => Type.Heli;

    [SerializeField]
    float stayTime;
    private float timer;
    
    private State state = State.CatchUp;

    protected override void ChangeSpeed()
    {
        Vector3 thisPos = this.transform.position;
        float sqrDist = thisPos.sqrMagnitude;
        Transform target = PlayerCar.current.Body;
        float baseSpeed = PlayerCar.current.SpeedMS;
        float angle = Vector3.Angle(target.forward, thisPos);

        switch (state)
        {
            case State.CatchUp:
                SpeedUp(baseSpeed);
                if (angle < 90 && sqrDist > myData.MovementData.StayLengthMin)
                {
                    StartStay();
                }
                break;
            case State.Stay:
                base.ChangeSpeed();
                timer -= Time.deltaTime;
                if (timer < 0)
                {
                    StartLeave();
                }
                break;
            case State.Leave:
                SpeedDown(baseSpeed);
                break;
            default:
                Debug.LogError($"このステートに対する処理がありません{state}");
                break;
        }
    }


    #region STATE_CHANGE
    private void StartStay()
    {
        state = State.Stay;
        timer = stayTime;
    }

    private void StartLeave()
    {
        state = State.Leave;
    }
    #endregion
}

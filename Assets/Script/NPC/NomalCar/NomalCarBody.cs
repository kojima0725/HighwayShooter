using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalCarBody : MonoBehaviour
{
    [SerializeField]
    private NomalCar car;

    public void DeadPush(Vector3 move)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.useGravity = true;
        body.constraints = RigidbodyConstraints.None;
        body.drag = 0.5f;
        body.velocity = move;
        Vector3 rotate = KMath.RandomVector(-7.0f, 7.0f);
        body.AddTorque(rotate, ForceMode.VelocityChange);
    }
}

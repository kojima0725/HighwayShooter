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
        Vector3 rotate = new Vector3(Random10, Random10, Random10);
        body.AddTorque(rotate, ForceMode.VelocityChange);
    }

    private float Random10 => Random.Range(-5.0f, 5.0f);
}

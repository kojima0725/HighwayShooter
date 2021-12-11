using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliRoller : MonoBehaviour
{
    [SerializeField]
    SnapAxis axis;
    [SerializeField]
    float speed;

    private void Update()
    {
        float rotate = speed * Time.deltaTime;
        if (axis == SnapAxis.X)
        {
            this.transform.Rotate(rotate, 0, 0);
        }
        if (axis == SnapAxis.Y)
        {
            this.transform.Rotate(0, rotate, 0);
        }
        if (axis == SnapAxis.Z)
        {
            this.transform.Rotate(0, 0, rotate);
        }
    }
}

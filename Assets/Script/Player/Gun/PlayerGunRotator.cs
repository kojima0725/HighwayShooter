using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunRotator : MonoBehaviour
{
    [SerializeField]
    float right;
    [SerializeField]
    float left;
    [SerializeField]
    float up;
    [SerializeField]
    float down;

    [SerializeField]
    PlayerGun gun;

    private void Update()
    {
        Vector2 pos = gun.GetReticlePos();
        Vector3 rotate = Vector3.zero;
        if (pos.y > 0)
        {
            rotate.x = -pos.y * up;
        }
        else
        {
            rotate.x = -pos.y * down;
        }
        if (pos.x > 0)
        {
            rotate.y = pos.x * right;
        }
        else
        {
            rotate.y = pos.x * left;
        }

        this.transform.localEulerAngles = rotate;
    }
}

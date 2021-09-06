using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NCar : MonoBehaviour
{
    protected float hp;

    public void GetDamage(float damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("死");
    }
}

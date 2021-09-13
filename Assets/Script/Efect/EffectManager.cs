using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    [SerializeField]
    GameObject bulletInpactRoad;
    public GameObject MakeBulletInpactRoad()
    {
        return Instantiate(bulletInpactRoad);
    }
    [SerializeField]
    GameObject bulletInpactCar;
    public GameObject MakeBulletInpactCar()
    {
        return Instantiate(bulletInpactCar);
    }

    [SerializeField]
    GameObject boom;
    public GameObject MakeBoom()
    {
        return Instantiate(boom);
    }


    [SerializeField]
    GameObject muzzleFlash;
    public GameObject MakeMuzzleFlash()
    {
        return Instantiate(muzzleFlash);
    }

    [SerializeField]
    GameObject smoke;
    public GameObject MakeSmoke()
    {
        return Instantiate(smoke);
    }
}

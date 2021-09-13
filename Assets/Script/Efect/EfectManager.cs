using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EfectManager : MonoBehaviour
{
    public static EfectManager instance;

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
    GameObject muzzleFlash;
    public GameObject MakeMuzzleFlash()
    {
        return Instantiate(muzzleFlash);
    }
}

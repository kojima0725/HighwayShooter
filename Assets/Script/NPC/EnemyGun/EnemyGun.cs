using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    const float FIRST_INTERVAL = 6.0f;
    [SerializeField]
    private Transform gunFrom;
    [SerializeField]
    private EnemyCar car;

    private EnemyCarData data;
    private float IntervalTimer;
    private GameObject muzzleFlash;

    private void Start()
    {
        data = car.CarData;
        IntervalTimer = FIRST_INTERVAL;
        muzzleFlash = EfectManager.instance.MakeMuzzleFlash();
        muzzleFlash.transform.parent = gunFrom;
        muzzleFlash.transform.localPosition = Vector3.zero;
        muzzleFlash.transform.localRotation = Quaternion.identity;
        muzzleFlash.SetActive(false);
    }

    private void Update()
    {
        IntervalTimer -= Time.deltaTime;
        if (IntervalTimer < 0)
        {
            StartCoroutine(ShootGun());
            IntervalTimer = data.GunShootInterval;
        }
    }

    private IEnumerator ShootGun()
    {
        muzzleFlash.SetActive(true);
        for (int i = 0; i < data.GunShooAtOnce; i++)
        {
            Shoot();
            yield return new WaitForSeconds(data.GunSpeed);
        }
        muzzleFlash.SetActive(false);
    }

    private void Shoot()
    {
        Vector3 targetPos = Vector3.zero;
        if (!PlayerCar.current)
        {
            targetPos = PlayerCar.current.transform.position;
        }
        RaycastHit hit;
        Ray ray = MakeGunRay(data.GunRange, data.GunAccurate, targetPos);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * data.GunRange, Color.white);
        if (Physics.Raycast(ray, out hit, data.GunRange))
        {
            GameObject hitEfect;
            if (hit.transform.tag == "Player")
            {
                Debug.Log("playerHit");
                hitEfect = EfectManager.instance.MakeBulletInpactCar();
            }
            else
            {
                hitEfect = EfectManager.instance.MakeBulletInpactRoad();
            }

            if (hitEfect)
            {
                hitEfect.transform.parent = hit.transform;
                hitEfect.transform.position = hit.point;
                hitEfect.transform.rotation = Quaternion.LookRotation(hit.normal);
            }

        }
    }


    /// <summary>
    /// 精度を考慮したレイを作成する
    /// </summary>
    /// <param name="range"></param>
    /// <param name="aculate"></param>
    /// <returns></returns>
    private Ray MakeGunRay(float range, float aculate, Vector3 target)
    {
        Vector3 dir = target - gunFrom.position;
        dir = dir.normalized * range;
        dir += KMath.RandomVector(-aculate, aculate);
        return new Ray(gunFrom.position, dir);
    }
}

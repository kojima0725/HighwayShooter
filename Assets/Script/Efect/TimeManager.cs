using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム中の時間の管理を行う
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    float timer;
    bool isSrowDown;
    float timeSpeed;
    float timeSpeedTarget;
    float timeChangeSpeed;

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
        timeSpeed = 1;
    }

    private void Start()
    {
        SoundEffectManager.instance.Pitch = 1;
    }
    public void SetTimeScale(float scale, float time, float changeSpeed = 2)
    {
        timer = time;
        isSrowDown = true;
        timeSpeedTarget = scale;
    }

    private void Update()
    {
        if (isSrowDown)
        {
            timer -= Time.deltaTime;
            timeSpeed = KMath.GetCloser(timeSpeed, timeSpeedTarget, timeChangeSpeed);
            if (timer < 0)
            {
                isSrowDown = false;
            }
        }
        else
        {
            timeSpeed = KMath.GetCloser(timeSpeed, 1, timeChangeSpeed);
            timeChangeSpeed = 2;
        }
        Time.timeScale = timeSpeed;
        SoundEffectManager.instance.Pitch = timeSpeed;

    }
}

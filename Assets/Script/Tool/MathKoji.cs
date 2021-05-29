using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class MathKoji
{
    /// <summary>
    /// ある値をある値に近づける
    /// </summary>
    /// <param name="current">現在</param>
    /// <param name="to">目標</param>
    /// <param name="rate">どれぐらい近づけるか(時間を考慮する場合一秒でどれぐらい動くか)</param>
    /// <param name="time">時間を考慮するか</param>
    /// <returns>近づけた結果</returns>
    public static float GetCloser(float current, float to, float rate, bool time = true)
    {
        if (time)
        {
            rate *= Time.deltaTime;
        }
        if (current < to)
        {
            current += rate;
            if (current > to)
            {
                return to;
            }
            else
            {
                return current;
            }
        }
        else if (current > to)
        {
            current -= rate;
            if (current < to)
            {
                return to;
            }
            else
            {
                return current;
            }
        }
        else
        {
            return current;
        }
    }

    /// <summary>
    /// 時速キロメートルを秒速メートルに変換して返す
    /// </summary>
    /// <param name="kmH">時速(キロメートル)</param>
    /// <returns>秒速(メートル)</returns>
    public static float KmHToMS(float kmH)
    {
        return kmH * 10 / 36;
    }

    /// <summary>
    /// 子のすべてのレイヤーを変える
    /// </summary>
    /// <param name="obj">変えるオブジェ</param>
    public static void SetLayer(GameObject obj, int setLayer)
    {
        obj.layer = setLayer;
        foreach (Transform transform in obj.transform)
        {
            SetLayer(transform.gameObject, setLayer);
        }
    }

    /// <summary>
    /// 確率生成器(UnityEngine.Random)を使用
    /// </summary>
    /// <param name="percent">trueの確率(0～100)</param>
    /// <returns>当たった場合true</returns>
    public static bool RandomBool(int percent)
    {
        return UnityEngine.Random.Range(1, 101) <= percent;
    }
}

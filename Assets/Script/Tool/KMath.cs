using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class KMath
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
    public static float KmHToMS(float kmH) => kmH * 10 / 36;

    /// <summary>
    /// 子のすべてのレイヤーを変える
    /// </summary>
    /// <param name="obj">変えるオブジェ</param>
    /// <param name="setLayer">設定するレイヤー</param>
    public static void SetLayer(GameObject obj, int setLayer)
    {
        obj.layer = setLayer;
        foreach (Transform transform in obj.transform)
        {
            SetLayer(transform.gameObject, setLayer);
        }
    }

    /// <summary>
    /// 子のすべてのタグを変える
    /// </summary>
    /// <param name="obj">変えるオブジェ</param>
    /// <param name="setTag">設定するタグ</param>
    public static void SetTag(GameObject obj, string setTag)
    {
        obj.tag = setTag;
        foreach (Transform transform in obj.transform)
        {
            SetTag(transform.gameObject, setTag);
        }
    }

    public static Transform GetRoot(Transform transform, string tagName)
    {
        while (transform.parent)
        {
            if (transform.parent.tag != tagName)
            {
                return transform;
            }
            transform = transform.parent;
        }
        return transform;
    }

    /// <summary>
    /// 確率生成器(UnityEngine.Randomを使用)
    /// </summary>
    /// <param name="percent">trueの確率(0～100)</param>
    /// <returns>当たった場合true</returns>
    public static bool RandomBool(int percent)
    {
        return UnityEngine.Random.Range(1, 101) <= percent;
    }

    /// <summary>
    /// 線分同士の当たり判定を行う
    /// </summary>
    /// <param name="l1s">線分１の始点</param>
    /// <param name="l1e">線分１の終点</param>
    /// <param name="l2s">線分２の始点</param>
    /// <param name="l2e">線分２の終点</param>
    /// <param name="hitPos">衝突地点の座標</param>
    /// <returns>衝突した場合はtrue</returns>
    public static bool LineToLineCollision(Vector2 l1s, Vector2 l1e, Vector2 l2s, Vector2 l2e, out Vector2 hitPos)
    {
        hitPos = Vector2.zero;

        var d = (l1e.x - l1s.x) * (l2e.y - l2s.y) - (l1e.y - l1s.y) * (l2e.x - l2s.x);

        if (d == 0.0f)
        {
            return false;
        }

        var u = ((l2s.x - l1s.x) * (l2e.y - l2s.y) - (l2s.y - l1s.y) * (l2e.x - l2s.x)) / d;
        var v = ((l2s.x - l1s.x) * (l1e.y - l1s.y) - (l2s.y - l1s.y) * (l1e.x - l1s.x)) / d;

        if (u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f)
        {
            return false;
        }

        hitPos.x = l1s.x + u * (l1e.x - l1s.x);
        hitPos.y = l1s.y + u * (l1e.y - l1s.y);

        return true;
    }

    public static bool LineToLineCollision(Vector2Line one, Vector2Line two, out Vector2 hitPos)
    {
        return LineToLineCollision(one.start, one.end, two.start, two.end, out hitPos);
    }

    /// <summary>
    /// Vector3のx,z座標をVector2に変換する拡張メソッド
    /// </summary>
    /// <param name="vector">変換元</param>
    /// <returns></returns>
    public static Vector2 ToVector2XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }


    /// <summary>
    /// Vector2のx,y座標をVector3のx,z座標に変換する拡張メソッド
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 ToVector3XZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0, vector.y);
    }
}

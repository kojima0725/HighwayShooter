using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 道路本体、自動で生成され続ける
/// </summary>
public class RoadChip : MonoBehaviour
{

    /// <summary>
    /// 自身の最後端の位置
    /// </summary>
    [SerializeField]
    private Transform end;

    /// <summary>
    /// メッシュフィルター
    /// </summary>
    [SerializeField]
    private MeshFilter meshFilter;


    /// <summary>
    /// 道の終端の位置を渡す
    /// </summary>
    /// <returns></returns>
    public Transform GetEnd()
    {
        return end;
    }

    /// <summary>
    /// 道路の初期化(メッシュの設置、各種調整)
    /// </summary>
    /// <param name="rotate">生成位置からどれぐらい曲げるか</param>
    /// <param name="length">チップの長さ</param>
    /// <param name="width">チップの幅</param>
    public void Init(Vector3 rotate,float length,float width)
    {
        //End位置の設定
        end.localPosition = new Vector3(0,0,length);

        //メッシュ生成用意
        Mesh mesh = new Mesh();
        //頂点生成
        Vector3[] vertices = new Vector3[4];
        float halfWidth = width / 2;
        float cosWidth = Mathf.Cos(Mathf.Deg2Rad * rotate.y) * halfWidth;
        float sinWidth = Mathf.Sin(Mathf.Deg2Rad * rotate.y) * halfWidth;

        vertices[0] = new Vector3(-cosWidth, 0, -sinWidth);
        vertices[1] = new Vector3(cosWidth, 0, sinWidth);
        vertices[2] = new Vector3(-halfWidth, 0, length);
        vertices[3] = new Vector3(halfWidth, 0, length);

        mesh.vertices = vertices;

        mesh.triangles = new int [] { 0, 2, 1, 1, 2, 3 };

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        //自身を曲げる
        this.transform.Rotate(rotate);
    }
}

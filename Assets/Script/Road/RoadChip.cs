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
    /// 右側のガードレール
    /// </summary>
    [SerializeField]
    private Transform guardrailLeft;
    /// <summary>
    /// 左側のガードレール
    /// </summary>
    [SerializeField]
    private Transform guardrailRight;

    /// <summary>
    /// メッシュフィルター
    /// </summary>
    [SerializeField]
    private MeshFilter meshFilter;

    /// <summary>
    /// 各レーンの位置
    /// </summary>
    private Transform[] lanes;

    /// <summary>
    /// 自身の次のチップ
    /// </summary>
    private RoadChip nextChip = null;


    /// <summary>
    /// 道の終端の位置を渡す
    /// </summary>
    /// <returns></returns>
    public Transform GetEnd()
    {
        return end;
    }

    /// <summary>
    /// 自身の次のロードチップを設定する
    /// </summary>
    /// <param name="chip"></param>
    public void SetNext(RoadChip chip)
    {
        nextChip = chip;
    }
    
    /// <summary>
    /// 次のロードチップを渡す(ない場合はnull)
    /// </summary>
    /// <returns></returns>
    public RoadChip GetNext()
    {
        return nextChip;
    }

    /// <summary>
    /// 指定されたレーンの位置を返す
    /// </summary>
    /// <param name="lane">レーンの番号(0～)</param>
    /// <returns></returns>
    public Transform GetLanePos(int lane)
    {
        return lanes[lane % lanes.Length];
    }

    /// <summary>
    /// 道路の初期化(メッシュの設置、各種調整)
    /// </summary>
    /// <param name="rotate">生成位置からどれぐらい曲げるか</param>
    /// <param name="length">チップの長さ</param>
    /// <param name="width">チップの幅</param>
    /// <param name="lane">車線数</param>
    public void Init(Vector3 rotate, float length, float width, int lane)
    {
        //End位置の設定
        end.localPosition = new Vector3(0,0,length);

        float halfWidth = width / 2;
        float halfLength = length / 2;

        //道路のメッシュ作成
        MakeMesh(rotate, length, halfWidth);

        //ガードレールの設置
        MakeGuardrail(halfLength, halfWidth, length);

        //レーンに対応した位置を作成
        MakeLanes(lane, halfWidth, length);

        //自身を曲げる
        this.transform.Rotate(rotate);
    }

    /// <summary>
    /// 道路のメッシュを作成する
    /// </summary>
    /// <param name="rotate"></param>
    /// <param name="length"></param>
    /// <param name="halfWidth"></param>
    private void MakeMesh(Vector3 rotate, float length, float halfWidth)
    {
        //メッシュ生成用意
        Mesh mesh = new Mesh();
        //頂点生成
        Vector3[] vertices = new Vector3[4];
        float cosWidth = Mathf.Cos(Mathf.Deg2Rad * rotate.y) * halfWidth;
        float sinWidth = Mathf.Sin(Mathf.Deg2Rad * rotate.y) * halfWidth;

        vertices[0] = new Vector3(-cosWidth, 0, -sinWidth);
        vertices[1] = new Vector3(cosWidth, 0, sinWidth);
        vertices[2] = new Vector3(-halfWidth, 0, length);
        vertices[3] = new Vector3(halfWidth, 0, length);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 2, 1, 1, 2, 3 };

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    /// <summary>
    /// ガードレールの設定をする
    /// </summary>
    /// <param name="halfLength"></param>
    /// <param name="halfWidth"></param>
    /// <param name="length"></param>
    private void MakeGuardrail(float halfLength, float halfWidth, float length)
    {
        guardrailLeft.localPosition = new Vector3(-halfWidth, 0, halfLength);
        guardrailLeft.localScale = new Vector3(1, 1, length);
        guardrailRight.localPosition = new Vector3(halfWidth, 0, halfLength);
        guardrailRight.localScale = new Vector3(1, 1, length);
    }

    /// <summary>
    /// 車線を作る
    /// </summary>
    /// <param name="lane"></param>
    private void MakeLanes(int lane, float halfWidth, float length)
    {
        //レーン作成
        lanes = new Transform[lane];

        float halfLaneWidth = halfWidth / lane;
        float LaneWidth = halfLaneWidth * 2;
        float pos = halfWidth - halfLaneWidth;

        for (int i = 0; i < lane; i++)
        {
            Transform laneTransform = new GameObject().transform;
            laneTransform.parent = this.transform;
            laneTransform.localPosition = new Vector3(pos,0,length);
            lanes[i] = transform;

            pos -= LaneWidth;
        }
    }
}

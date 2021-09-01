﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapChip : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;

    float width;
    int split;
    Vector2 noizeStartPos;
    Vector2 front;
    Vector2 sNomal;
    Vector2 eNomal;
    MapData data;
    /// <summary>
    /// 地形の起伏を何倍するか
    /// </summary>
    float bai;
    Transform end;

    /// <summary>
    /// マップチップの一番端っこ
    /// </summary>
    public Transform End => end;

    public void Init(Vector2 front, MapData data, Vector2 sNomal, Vector3 eNomal, bool LR, MapNoizeManager noizeManager, List<Transform> makedLine)
    {
        this.front = front;
        this.width = data.ChipWidth;
        this.split = data.SplitX;
        this.noizeStartPos = noizeManager.noiseStartPos;
        this.sNomal = sNomal;
        this.eNomal = eNomal;
        this.data = data;
        MakeMesh(LR, noizeManager, makedLine);
    }

    private void MakeMesh(bool LR, MapNoizeManager noize, List<Transform> makedLine)
    {
        //メッシュ生成
        Mesh mesh = new Mesh();

        //頂点生成開始
        int xCount = split + 1;
        int verticsCount = xCount * 2;
        float polyWidth = width / split;

        //すでに作成されている道路とかぶらないように長さを調節する
        Vector2 start1 = this.transform.position.ToVextor2XZ();
        Vector2 hit1 = start1;
        Vector2 start2 = this.transform.position.ToVextor2XZ() + front;
        Vector2 hit2 = start2;
        if (HitCheck(hit1, hit1 + sNomal * width, makedLine, out hit1) || HitCheck(hit2, hit2 + eNomal * width, makedLine, out hit2))
        {
            float a = (start1 - hit1).sqrMagnitude;
            float b = (start2 - hit2).sqrMagnitude;
            width = Mathf.Sqrt(Mathf.Max(a,b));
            xCount = (int)(width / polyWidth);
            split = xCount - 1;
        }


        float length = 0;
        Vector3[] vertices = new Vector3[verticsCount];
        Vector2[] uvs = new Vector2[verticsCount];
        int index = 0;
        Vector2 pos = Vector2.zero;
        float uvW = 0;
        for (int i = 0; i < xCount; i++)
        {
            if (length <= data.Height.length)
            {
                bai = length / data.Height.length * data.Height.height;
            }
            else
            {
                bai = data.Height.height + (length - data.Height.length) / width * (1.0f - data.Height.height);
            }
            vertices[index] = new Vector3(pos.x, Noize(pos.x, pos.y), pos.y);
            uvs[index] = new Vector2(uvW, 0);
            pos += sNomal * polyWidth;
            length += polyWidth;
            uvW++;
            index++;
        }
        

        noize.Move(front);

        length = 0;
        pos = front;
        uvW = 0;
        for (int i = 0; i < xCount; i++)
        {
            if (length <= data.Height.length)
            {
                bai = length / data.Height.length * data.Height.height;
            }
            else
            {
                bai = data.Height.height + (length - data.Height.length) / width * (1.0f - data.Height.height);
            }
            vertices[index] = new Vector3(pos.x, Noize(pos.x, pos.y), pos.y);
            uvs[index] = new Vector2(uvW, 1);
            pos += eNomal * polyWidth;
            length += polyWidth;
            uvW++;
            index++;
        }
        mesh.vertices = vertices;
        mesh.uv = uvs;

        int[] triangles = new int[6 * split];
        int tIndex = 0;
        for (int y = 0; y < split; y++)
        {
            if (LR)
            {
                Inport(y);
                Inport(y + split + 2);
                Inport(y + split + 1);
                Inport(y);
                Inport(y + 1);
                Inport(y + split + 2);
            }
            else
            {
                Inport(y);
                Inport(y + split + 1);
                Inport(y + 1);
                Inport(y + 1);
                Inport(y + split + 1);
                Inport(y + split + 2);
            }
        }
        void Inport(int c)
        {
            triangles[tIndex] = c;
            tIndex++;
        }


        mesh.triangles = triangles;

        // 領域と法線を自動で再計算する
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;

        //チップの端の位置情報を作る
        end = new GameObject("end").transform;
        end.parent = this.transform;
        end.localPosition = vertices[vertices.Length - 1];
    }

    private float Noize( float x, float y)
    {
        Vector2 pos = MapNoizeManager.NoizePos(noizeStartPos, x, y);
        return (Mathf.PerlinNoise(pos.x, pos.y) - data.Down) * data.MaxHeight * bai;
    }

    /// <summary>
    /// 線と線の当たり判定をチェックする
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    private bool HitCheck(Vector2 start, Vector2 end, List<Transform> line, out Vector2 hitPos)
    {
        hitPos = start;
    }
}

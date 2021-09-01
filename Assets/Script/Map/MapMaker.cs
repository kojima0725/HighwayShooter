using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private MapChip mapChipPrefab;

    private MapNoizeManager noizeManagerL;
    private MapNoizeManager noizeManagerR;

    private Vector2 lastChipLNomal;
    private Vector2 lastChipRNomal;

    private List<Transform> makeEndLine = new List<Transform>();
    private bool CheckLR;

    private void Awake()
    {
        noizeManagerL = new MapNoizeManager();
        noizeManagerR = new MapNoizeManager();
    }

    private void Update()
    {
        for (int i = 0; i < makeEndLine.Count - 1; i++)
        {
            Debug.DrawLine(makeEndLine[i].position, makeEndLine[i + 1].position);
        }
    }

    /// <summary>
    /// チップの左右にマップのポリゴンを作成する
    /// </summary>
    /// <param name="chips"></param>
    /// <param name="haveLR">左右に曲がっているか</param>
    /// <param name="LR">曲がっている方向(L = true, R = false)</param>
    public void MakeMap(List<RoadChip> chips, bool haveLR, bool LR)
    {
        //どこまで作ったかのLINEを設定するときの左右判定
        if (haveLR)
        {
            CheckLR = LR;
        }


        MapData d = StageDatabase.MapData;
        int listLength = chips.Count;
        for (int i = 0; i < listLength; i++)
        {
            RoadChip item = chips[i];
            Vector3[] wa = item.Vertices;
            Vector2[] wb = new Vector2[4];
            for (int j = 0; j < 4; j++)
            {
                wb[j] = new Vector2(wa[j].x, wa[j].z);
            }

            MapChip maked = Instantiate(mapChipPrefab);
            item.SetMap(true, maked);
            Vector2 leftGurd = new Vector2(item.GurdrailLeftVector.x, item.GurdrailLeftVector.z);
            maked.Init(leftGurd, d, lastChipLNomal, -item.GurdrailLeftNomal, true, noizeManagerL);
            lastChipLNomal = -item.GurdrailLeftNomal;

            maked = Instantiate(mapChipPrefab);
            item.SetMap(false, maked);
            Vector2 rightGurd = new Vector2(item.GurdrailRightVector.x, item.GurdrailRightVector.z);
            maked.Init(rightGurd, d, lastChipRNomal , -item.GurdrailRightNomal, false, noizeManagerR);
            lastChipRNomal = -item.GurdrailRightNomal;
        }

        RoadChip end = chips.Last();
        MakeEndLine(end.MapChipLEnd, end.MapChipREnd);
    }

    private void MakeEndLine(Transform l, Transform r)
    {
        List<Transform> currentLine = makeEndLine;
        if (currentLine.Count == 0)
        {
            makeEndLine.Add(l);
            makeEndLine.Add(r);
            return;
        }

        Transform start = null;
        Transform end = null;
        int startIndex = 0;
        int endIndex = 0;
        int next = 0;
        switch (CheckLR)
        {
            case true:
                start = r;
                end = l;
                startIndex = currentLine.Count - 1;
                endIndex = 0;
                next = -1;
                break;
            case false:
                start = l;
                end = r;
                startIndex = 0;
                endIndex = currentLine.Count - 1;
                next = 1;
                break;
        }
        int index = startIndex;
        bool nothit = true;
        while (index != endIndex)
        {
            Vector2 hitPos;
            if (KMath.LineToLineCollision(start.position.ToVextor2XZ(), end.position.ToVextor2XZ(), currentLine[index].position.ToVextor2XZ(), currentLine[index + next].position.ToVextor2XZ(), out hitPos))
            {
                nothit = false;
                makeEndLine = new List<Transform>();
                makeEndLine.Add(start);
                Transform hit = new GameObject("hit").transform;
                hit.parent = start;
                hit.position = new Vector3(hitPos.x, 0 ,hitPos.y);
                makeEndLine.Add(hit);
                while (index != endIndex)
                {
                    makeEndLine.Add(currentLine[index + next]);
                    index += next;
                }

                if (CheckLR)
                {
                    makeEndLine.Reverse();
                }
                break;
            }
            index += next;
        }
        if (nothit)
        {
            makeEndLine = new List<Transform>();
            makeEndLine.Add(l);
            makeEndLine.Add(r);
        }
    }
}

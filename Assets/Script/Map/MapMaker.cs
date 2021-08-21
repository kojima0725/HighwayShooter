using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [SerializeField]
    private MapChip mapChipPrefab;

    private MapNoizeManager noizeManagerL;
    private MapNoizeManager noizeManagerR;

    private Vector2 l;
    private Vector2 r;

    private void Awake()
    {
        noizeManagerL = new MapNoizeManager();
        noizeManagerR = new MapNoizeManager();
    }

    public void MakeMap(List<RoadChip> chips)
    {
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
            Vector2 leftGurd = new Vector2(item.GurdrailLeftVector.x, item.GurdrailLeftVector.z);
            maked.Init(leftGurd, d, l, -item.GurdrailLeftNomal, true, noizeManagerL);
            item.SetMap(true, maked);
            l = -item.GurdrailLeftNomal;

            maked = Instantiate(mapChipPrefab);
            Vector2 rightGurd = new Vector2(item.GurdrailRightVector.x, item.GurdrailRightVector.z);
            maked.Init(rightGurd, d, r , -item.GurdrailRightNomal, false, noizeManagerR);
            item.SetMap(false, maked);
            r = -item.GurdrailRightNomal;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI current;

    [SerializeField]
    private Text text;

    private void Awake()
    {
        current = this;
    }

    private void OnDestroy()
    {
        current = null;
    }

    public void UpdateScore(int score)
    {
        text.text = $"撃破数:{score}";
    }
}

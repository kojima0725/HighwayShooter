using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    private static int score = 0;

    public static void ScoreUp()
    {
        score++;
        UpdateUI();
    }

    public static void StartGame()
    {
        score = 0;
        UpdateUI();
    }

    private static void UpdateUI()
    {
        if (ScoreUI.current)
        {
            ScoreUI.current.UpdateScore(score);
        }
    }
}

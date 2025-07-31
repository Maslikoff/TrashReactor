using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    public void SubmitTimeToLeaderboard(float timeInSeconds)
    {
        int score = Mathf.RoundToInt(timeInSeconds * 1000);

        YG2.SetLeaderboard("TechnoNameLB", score);
    }
}
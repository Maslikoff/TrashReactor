using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] timerTexts;

    public void UpdateTimeDisplay(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);

        foreach (var timer in timerTexts)
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
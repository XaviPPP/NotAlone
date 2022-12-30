using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    // Update is called once per frame
    void Update()
    {
        int hours = Enviro.EnviroManager.instance.Time.hours;
        int minutes = Enviro.EnviroManager.instance.Time.minutes;

        string hoursText = hours.ToString();
        string minutesText = minutes.ToString();

        if (hours < 10)
        {
            hoursText = $"0{hours}";
        }

        if (minutes < 10)
        {
            minutesText = $"0{minutes}";
        }

        timeText.text = $"{hoursText}:{minutesText}";
    }
}

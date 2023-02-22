using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[HideMonoScript]
public class TimeUpdater : MonoBehaviour
{
    [Title("UI")]
    [Indent][SerializeField] private TextMeshProUGUI timeText;
    [Indent][SerializeField] private bool timeFormatTo24Hour = true;

    // Update is called once per frame
    void Update()
    {
        int hours = Enviro.EnviroManager.instance.Time.hours;
        int minutes = Enviro.EnviroManager.instance.Time.minutes;

        string hoursText = hours.ToString();
        string minutesText = minutes.ToString();

        if (timeFormatTo24Hour)
        {
            if (hours < 10)
            {
                hoursText = $"0{hours}";
            }

            if (minutes < 10)
            {
                minutesText = $"0{minutes}";
            }
        }

        timeText.text = $"{hoursText}:{minutesText}";
    }
}

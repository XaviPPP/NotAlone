using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsChanger : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown modeDropdown;

    void Start()
    {
        graphicsDropdown.value = GetActualGraphicQualityLevel();
        modeDropdown.value = GetActualMode();
    }

    public void ApplySettings()
    {
        ChangeQualityLevel();
        ChangeResolution();
    }

    public void ChangeQualityLevel()
    {
        if (graphicsDropdown.value == 0)
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (graphicsDropdown.value == 1)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (graphicsDropdown.value == 2)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
    }

    public void ChangeResolution()
    {
        switch (resolutionDropdown.value)
        {
            case 0:
                Screen.SetResolution(2560, 1440, GetMode());
                break;
            case 1:
                Screen.SetResolution(1920, 1080, GetMode());
                break;
            case 2:
                Screen.SetResolution(1366, 768, GetMode());
                break;
            case 3:
                Screen.SetResolution(1280, 720, GetMode());
                break;
            case 4:
                Screen.SetResolution(1024, 768, GetMode());
                break;
            case 5:
                Screen.SetResolution(800, 600, GetMode());
                break;
        }
    }

    private int GetActualGraphicQualityLevel()
    {
        return QualitySettings.GetQualityLevel();
    }

    private int GetActualMode()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                return 0;
            case FullScreenMode.Windowed:
                return 1;
        }
        return 0;
    }

    private FullScreenMode GetMode()
    {
        switch (modeDropdown.value)
        {
            case 0:
                return FullScreenMode.ExclusiveFullScreen;
            case 1:
                return FullScreenMode.Windowed;
        }
        return FullScreenMode.Windowed;
    }
}

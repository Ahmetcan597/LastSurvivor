using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    public TextMeshProUGUI screenResolution;
    string changableReso;
    int starter = 0;
    string[] realScreen = new string[2];
    public void Start()
    {

        fullscreenTog.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
    }

    public void Update()
    {
        switch (starter)
        {
            case 1:
                changableReso = "1366x768";
                screenResolution.SetText(changableReso);
                break;
            case 2:
                changableReso = "1920x1080";
                screenResolution.SetText(changableReso);
                break;
            case 3:
                changableReso = "2560x1440";
                screenResolution.SetText(changableReso);
                break;
            case 4:
                changableReso = "3840x2160";
                screenResolution.SetText(changableReso);
                break;
        }
    }

    public void UpResolution()
    {
        if(starter < 4)
        {
            starter++;
        }

    }
    public void DownResolution()
    {
        if(starter > 0)
        {
            starter--;
        }
    }



    public void ApplyGraphics()
    {
        realScreen = screenResolution.text.Split('x');
        Screen.SetResolution(int.Parse(realScreen[0]), int.Parse(realScreen[1]), fullscreenTog.isOn);

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

    }

}

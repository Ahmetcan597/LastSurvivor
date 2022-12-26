using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowDownLength = 2f;
    public float upDownLength = 2f;
    private bool check = false;
    private bool flag = false;

    void Update()
    {
        if (check && flag)
        {
            Time.timeScale += (1f / upDownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.3f, 1f);
            if (Time.timeScale == 1f)
            {
                flag = false;
                check = false;
            }
        }
        else if(!check && flag)
        {
            Time.timeScale -= (1f / slowDownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0.3f, 1f);
            if(Time.timeScale == 0.3f)
            {
                check = true;
            }
        }
    }

    public void DoSlowmotion()
    {
        flag = true;
    }
}

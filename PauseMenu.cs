using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject GamePause;

    public void Resume()
    {
        print("Calisti");
        Time.timeScale = 1f;
        GamePause.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(2);
    }
    public void Options()
    {
        //SceneManager.LoadScene(2);
    }
}

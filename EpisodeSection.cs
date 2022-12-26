using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpisodeSection : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene(4);
    }

    public void Level2()
    {
        SceneManager.LoadScene(2);
    }
}

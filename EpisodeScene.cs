using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EpisodeScene : MonoBehaviour
{
    public int whichEpisode = 1;
    // Start is called before the first frame update
    void Start()
    {
        switch (whichEpisode)
        {
            case 1:
                Invoke("exitEpisode",39.2f);
                break;
            case 2:
                Invoke("exitEpisode", 12.2f);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            exitEpisode();
        }
    }

    void exitEpisode()
    {
        switch (whichEpisode)
        {
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(0);
                break;
        }
    }
}

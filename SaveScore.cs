using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveScore : MonoBehaviour
{
    [SerializeField] public int SceneNumber;
    public TextMeshProUGUI highScoreText;

    private void Start()
    {
        GetScore(SceneNumber);
    }

    void GetScore(int scene)
    {

        highScoreText.SetText("HighScore: " + PlayerPrefs.GetInt($"HighScore{scene}"));

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    public Slider volumeSlider;
    public GameObject objectMusic;
    private float musicVolume = 1f;
    void Start()
    {
        PlayerPrefs.SetFloat("volume", 0.5f);
        objectMusic = GameObject.FindGameObjectWithTag("GameMusic");
        audioSource = objectMusic.GetComponent<AudioSource>();
        musicVolume = PlayerPrefs.GetFloat("volume");
        audioSource.volume = musicVolume;
        volumeSlider.value = musicVolume;
    }

    void Update()
    {
        audioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("volume", musicVolume);
    }

    public void UpdateVolume(float volume)
    {
        musicVolume = volume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public GameObject main;
    public AudioMixer audioMixer;
    public AudioSource fxSource;
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
    private void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            gameObject.SetActive(false);
            fxSource.Play(0);
            main.SetActive(true);
        }

    }
}

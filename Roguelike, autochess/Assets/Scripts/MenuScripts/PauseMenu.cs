using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseUI;
    public GameObject UIcanvas;
    public GameObject HealthCanvas;
    public GameObject settingsMenu;
    public AudioSource fxSource;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (paused == true)
                Resume();
            else Pause();
        }
    }
    public void Resume()
    {
        if (!settingsMenu.activeSelf)
        {
            fxSource.Play(0);
            pauseUI.SetActive(false);
            UIcanvas.SetActive(true);
            HealthCanvas.SetActive(true);
            Time.timeScale = 1f;
            paused = false;
        }
       
    }
    public void Pause()
    {
        fxSource.Play(0);
        pauseUI.SetActive(true);
        UIcanvas.SetActive(false);
        HealthCanvas.SetActive(false);
        Time.timeScale = 1f;
        paused = true;
    }
    public void toMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}

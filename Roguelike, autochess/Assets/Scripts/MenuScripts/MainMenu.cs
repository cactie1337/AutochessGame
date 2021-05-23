using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public sc SceneChanger;
    public void quitButton()
    {
        Application.Quit();
    }
    public void playButton()
    {
        SceneChanger = FindObjectOfType(typeof(sc)) as sc;
        SceneChanger.ToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("Mountain");

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCheatMenuScript : MonoBehaviour
{
    public GameObject CheatMenu;
    public GameObject OpenButton;
    public void OpenCheatMenu()
    {
        OpenButton.SetActive(false);
        CheatMenu.SetActive(true);
    }
}

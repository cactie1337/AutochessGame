using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuKeyboard : MonoBehaviour
{
    public string key;

    public void Update()
    {
        if (Input.GetKeyDown(key))
            EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuObject
{
    public bool isActive;

    public Transform menuTransform;
    public Transform activePosition;
    public Transform hiddenPosition;

    public void Toggle()
    {
        if (isActive)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    public void Open()
    {
        //set it to its 'active' position
        menuTransform.position = activePosition.position;

        //set it to active
        isActive = true;
    }

    public void Close()
    {
        //set it to its 'hidden' position
        menuTransform.position = hiddenPosition.position;

        //set it to inactive
        isActive = false;
    }

}


public class UIManager : Singleton<UIManager>
{
    [Header("Unit HealthBars")]
    [SerializeField]
    private Transform unitHealthBarCanvas;

    public Transform UnitHealthBarCanvas { get => unitHealthBarCanvas; protected set => unitHealthBarCanvas = value; }
}

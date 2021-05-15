using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
    [SerializeField]
    private int currentGold;
    private UIManager uiManagerScript;

    public int CurrentGold { get => currentGold; protected set => currentGold = value; }
    public UIManager UIManagerScript { get => uiManagerScript; protected set => uiManagerScript = value; }

    public virtual void Awake()
    {
        UIManagerScript = UIManager.Instance;
        CurrentGold = 0;
    }
    public virtual bool SpendGold(int amount)
    {
        if(amount <= CurrentGold)
        {
            CurrentGold -= amount;
            //UIManagerScript.UpdateCurrentGoldText(CurrentGold);
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void GainGold(int amount)
    {
        CurrentGold += amount;
        //UIManagerScript.UpdateCurrentGoldText(CurrentGold);
    }
}

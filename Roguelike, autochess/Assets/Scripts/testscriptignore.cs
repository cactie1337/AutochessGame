using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscriptignore : MonoBehaviour
{
    public UnitStats b1;
    public UnitStats b2;
    public UnitStats i1;
    public UnitStats i2;
    public UnitStats s1;
    public UnitStats s2;

    public GameObject panel;
    public GameObject round;
    public GameObject active;
    public GameObject startCmb;
    public GameObject shop;

    private BenchManager benchManagerScript;
    private GoldManager goldManagerScript;

    public UnitStats B1 { get => b1; set => b1 = value; }
    public UnitStats B2 { get => b2; set => b2 = value; }
    public UnitStats I1 { get => i1; set => i1 = value; }
    public UnitStats I2 { get => i2; set => i2 = value; }
    public UnitStats S1 { get => s1; set => s1 = value; }
    public UnitStats S2 { get => s2; set => s2 = value; }
    protected BenchManager BenchManagerScript { get => benchManagerScript; set => benchManagerScript = value; }
    protected GoldManager GoldManagerScript { get => goldManagerScript; set => goldManagerScript = value; }

    protected virtual void Awake()
    {
        BenchManagerScript = BenchManager.Instance;
        GoldManagerScript = GoldManager.Instance;
    }
    public virtual void onClickBeasts()
    {
        if (B1 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(B1, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }
        
        if (B2 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(B2, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }
        panel.SetActive(false);
        makeActive();
    }
    public virtual void onClickInfernals()
    {
        if (I1 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(I1, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }

        if (I2 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(I2, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }
        panel.SetActive(false);
        makeActive();
    }
    public virtual void onClickShocktouch()
    {
        if (S1 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(S1, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }

        if (S2 == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if (GoldManagerScript.SpendGold(0))
            {
                if (!BenchManagerScript.AddNewUnitToBench(S2, 0))
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }
        panel.SetActive(false);
        makeActive();
    }
    void makeActive()
    {
        round.SetActive(true);
        active.SetActive(true);
        startCmb.SetActive(true);
        shop.SetActive(true);
    }
}

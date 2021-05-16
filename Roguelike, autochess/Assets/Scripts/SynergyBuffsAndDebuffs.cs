using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyBuffsAndDebuffs : Singleton<SynergyBuffsAndDebuffs>
{
    protected virtual void Awake()
    {
        InitializeAssassinSynergy();
    }

    [Header("Assassin synergy")]
    [SerializeField]
    private Synergy assassinSynergy;

    protected Synergy AssassinSynergy { get => assassinSynergy; set => assassinSynergy = value; }

    protected virtual void Empty(List<GameObject> pawns)
    {
        Debug.LogWarning("Called 'Empty' function in SynergyBuffsAndDebuffs script. Make sure your synergies are properly" +
            "initialized and setup.");
    }

    protected virtual void InitializeAssassinSynergy()
    {
        if(!AssassinSynergy)
        {
            return;
        }

        AssassinSynergy.buff1 = ASSASSIN_BUFF_1;
        AssassinSynergy.buff2 = Empty;
    }
    public virtual void ASSASSIN_BUFF_1(List<GameObject> units)
    {
        int damageBuff = 10;
        foreach(GameObject unit in units)
        {
            Unit unitScript = unit.GetComponent<Unit>();

            unitScript.Synergy_BonusAttackSpeed += damageBuff;
            unitScript.BonusAttackSpeed += damageBuff;
            unitScript.CalculateAllStats();
        }
    }
    
}

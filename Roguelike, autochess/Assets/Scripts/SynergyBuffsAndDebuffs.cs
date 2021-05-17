using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyBuffsAndDebuffs : Singleton<SynergyBuffsAndDebuffs>
{
    protected virtual void Awake()
    {
        InitializeAssassinSynergy();
        InitializeInfernalSynergy();
    }
    protected virtual void Empty(List<GameObject> pawns)
    {
        Debug.LogWarning("Called 'Empty' function in SynergyBuffsAndDebuffs script. Make sure your synergies are properly" +
            "initialized and setup.");
    }
    #region Assassin

    [Header("Assassin synergy")]
    [SerializeField]
    private Synergy assassinSynergy;

    protected Synergy AssassinSynergy { get => assassinSynergy; set => assassinSynergy = value; }

    protected virtual void InitializeAssassinSynergy()
    {
        if(!AssassinSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        AssassinSynergy.buff1 = ASSASSIN_BUFF_1;
        AssassinSynergy.buff2 = Empty;
        AssassinSynergy.buff3 = Empty;
    }
    public virtual void ASSASSIN_BUFF_1(List<GameObject> units)
    {
        int damageBuff = 10;
        foreach(GameObject unit in units)
        {
            Unit unitScript = unit.GetComponent<Unit>();

            unitScript.Synergy_BonusDamage += damageBuff;
            unitScript.BonusDamage += damageBuff;
            unitScript.CalculateAllStats();
        }
    }
    #endregion
    #region Infernal
    [Header("Infernal synergy")]
    [SerializeField]
    private Synergy infernalSynergy;
    protected Synergy InfernalSynergy { get => infernalSynergy; set => infernalSynergy = value; }

    protected virtual void InitializeInfernalSynergy()
    {
        if(!InfernalSynergy)
        {
            Debug.LogError("No 'InfernalSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        InfernalSynergy.buff1 = INFERNAL_BUFF_1;
        InfernalSynergy.buff2 = Empty;
        InfernalSynergy.buff3 = Empty;
    }
    public virtual void INFERNAL_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
}

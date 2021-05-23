using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyBuffsAndDebuffs : Singleton<SynergyBuffsAndDebuffs>
{
    protected virtual void Awake()
    {
        #region class
        InitializeAssassinSynergy();
        InitializeRangerSynergy();
        InitializeWizardSynergy();
        InitializeWarriorSynergy();
        InitializeSpecialistSynergy();
        InitializeHealerSynergy();
        InitializeCurserSynergy();
        InitializeSummonerSynergy();
        #endregion

        #region traits
        InitializeFrostBloodSynergy();
        InitializeInfernalSynergy();
        InitializeToxicSynergy();
        InitializeBeastSynergy();
        InitializeShockTouchSynergy();
        #endregion
    }
    protected virtual void Empty(List<GameObject> pawns)
    {
        Debug.LogWarning("Called 'Empty' function in SynergyBuffsAndDebuffs script. Make sure your synergies are properly" +
            "initialized and setup.");
    }
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
    #region Frostblood
    [Header("Frostblood")]
    [SerializeField]
    private Synergy frostbloodSynergy;
    protected Synergy FrostbloodSynergy { get => frostbloodSynergy; set => frostbloodSynergy = value; }

    protected virtual void InitializeFrostBloodSynergy()
    {
        if (!FrostbloodSynergy)
        {
            Debug.LogError("No 'InfernalSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        FrostbloodSynergy.buff1 = FROSTBLOOD_BUFF_1;
        FrostbloodSynergy.buff2 = Empty;
        FrostbloodSynergy.buff3 = Empty;
    }
    public virtual void FROSTBLOOD_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Toxic
    [Header("Toxic")]
    [SerializeField]
    private Synergy toxicSynergy;
    protected Synergy ToxicSynergy { get => toxicSynergy; set => toxicSynergy = value; }

    protected virtual void InitializeToxicSynergy()
    {
        if (!ToxicSynergy)
        {
            Debug.LogError("No 'InfernalSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        ToxicSynergy.buff1 = Toxic_BUFF_1;
        ToxicSynergy.buff2 = Empty;
        ToxicSynergy.buff3 = Empty;
    }
    public virtual void Toxic_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Beast
    [Header("Beast")]
    [SerializeField]
    private Synergy beastSynergy;
    protected Synergy BeastSynergy { get => beastSynergy; set => beastSynergy = value; }

    protected virtual void InitializeBeastSynergy()
    {
        if (!BeastSynergy)
        {
            Debug.LogError("No 'InfernalSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        BeastSynergy.buff1 = Beast_BUFF_1;
        BeastSynergy.buff2 = Empty;
        BeastSynergy.buff3 = Empty;
    }
    public virtual void Beast_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Shocktouch
    [Header("Shocktouch")]
    [SerializeField]
    private Synergy shocktouchSynergy;
    protected Synergy ShockTouchSynergy { get => shocktouchSynergy; set => shocktouchSynergy = value; }

    protected virtual void InitializeShockTouchSynergy()
    {
        if (!ShockTouchSynergy)
        {
            Debug.LogError("No 'InfernalSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        ShockTouchSynergy.buff1 = Shocktouch_BUFF_1;
        ShockTouchSynergy.buff2 = Empty;
        ShockTouchSynergy.buff3 = Empty;
    }
    public virtual void Shocktouch_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Ranger
    [Header("Ranger synergy")]
    [SerializeField]
    private Synergy rangerSynergy;
    protected Synergy RangerSynergy { get => rangerSynergy; set => rangerSynergy = value; }
    protected virtual void InitializeRangerSynergy()
    {
        if(!RangerSynergy)
        {
            Debug.LogError("No 'RangerSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }
        RangerSynergy.buff1 = RANGER_BUFF_1;
        RangerSynergy.buff2 = Empty;
        RangerSynergy.buff3 = Empty;
    }
    public virtual void RANGER_BUFF_1(List<GameObject> units)
    {
        
    }
    #endregion
    #region Assassin

    [Header("Assassin synergy")]
    [SerializeField]
    private Synergy assassinSynergy;

    protected Synergy AssassinSynergy { get => assassinSynergy; set => assassinSynergy = value; }

    protected virtual void InitializeAssassinSynergy()
    {
        if (!AssassinSynergy)
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
        foreach (GameObject unit in units)
        {
            Unit unitScript = unit.GetComponent<Unit>();

            unitScript.Synergy_BonusDamage += damageBuff;
            unitScript.BonusDamage += damageBuff;
            unitScript.CalculateAllStats();
        }
    }
    #endregion
    #region Wizard

    [Header("Wizard synergy")]
    [SerializeField]
    private Synergy wizardSynergy;

    protected Synergy WizardSynergy { get => wizardSynergy; set => wizardSynergy = value; }

    protected virtual void InitializeWizardSynergy()
    {
        if (!WizardSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        WizardSynergy.buff1 = WIZARD_BUFF_1;
        WizardSynergy.buff2 = Empty;
        WizardSynergy.buff3 = Empty;
    }
    public virtual void WIZARD_BUFF_1(List<GameObject> units)
    {
       
    }
    #endregion
    #region Warrior

    [Header("Warrior synergy")]
    [SerializeField]
    private Synergy warriorSynergy;

    protected Synergy WarriorSynergy { get => warriorSynergy; set => warriorSynergy = value; }

    protected virtual void InitializeWarriorSynergy()
    {
        if (!WarriorSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        WarriorSynergy.buff1 = WARRIOR_BUFF_1;
        WarriorSynergy.buff2 = Empty;
        WarriorSynergy.buff3 = Empty;
    }
    public virtual void WARRIOR_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Healer

    [Header("Healer synergy")]
    [SerializeField]
    private Synergy healerSynergy;

    protected Synergy HealerSynergy { get => healerSynergy; set => healerSynergy = value; }

    protected virtual void InitializeHealerSynergy()
    {
        if (!HealerSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        HealerSynergy.buff1 = HEALER_BUFF_1;
        HealerSynergy.buff2 = Empty;
        HealerSynergy.buff3 = Empty;
    }
    public virtual void HEALER_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Specialist

    [Header("Specialist synergy")]
    [SerializeField]
    private Synergy specialistSynergy;

    protected Synergy SpecialistSynergy { get => specialistSynergy; set => specialistSynergy = value; }

    protected virtual void InitializeSpecialistSynergy()
    {
        if (!SpecialistSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        SpecialistSynergy.buff1 = SPECIALIST_BUFF_1;
        SpecialistSynergy.buff2 = Empty;
        SpecialistSynergy.buff3 = Empty;
    }
    public virtual void SPECIALIST_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Summoner

    [Header("Summoner synergy")]
    [SerializeField]
    private Synergy summonerSynergy;

    protected Synergy SummonerSynergy { get => summonerSynergy; set => summonerSynergy = value; }

    protected virtual void InitializeSummonerSynergy()
    {
        if (!SummonerSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        SummonerSynergy.buff1 = SUMMONER_BUFF_1;
        SummonerSynergy.buff2 = Empty;
        SummonerSynergy.buff3 = Empty;
    }
    public virtual void SUMMONER_BUFF_1(List<GameObject> units)
    {

    }
    #endregion
    #region Curser

    [Header("Curser synergy")]
    [SerializeField]
    private Synergy curserSynergy;

    protected Synergy CurserSynergy { get => curserSynergy; set => curserSynergy = value; }

    protected virtual void InitializeCurserSynergy()
    {
        if (!CurserSynergy)
        {
            Debug.LogError("No 'AssassinSynergy' reference set on the SynergyBuffsAndDebuffs script on the GameManager gameobject. Please set a reference to this synergy before entering playmode!");
            return;
        }

        CurserSynergy.buff1 = CURSER_BUFF_1;
        CurserSynergy.buff2 = Empty;
        CurserSynergy.buff3 = Empty;
    }
    public virtual void CURSER_BUFF_1(List<GameObject> units)
    {

    }
    #endregion

}

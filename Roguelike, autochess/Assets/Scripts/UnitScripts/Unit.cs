using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targeting), typeof(AutoAttack), typeof(Movement))]
[RequireComponent(typeof(Status), typeof(HomeBase), typeof(HealthAndMana))]
public class Unit : MonoBehaviour
{
    [Header("BASE STATS")]
    [SerializeField]
    private UnitStats stats;

    [Header("ATK")]
    [Header("DO NOT ALTER HERE")]
    [Header("CALCULATED STATS")]
    [SerializeField]
    [Tooltip("Minimum dmg from an auto attack")]
    private int minAttackDamage;
    [SerializeField]
    [Tooltip("Maximum dmg from an auto attack")]
    private int maxAttackDamage;
    [SerializeField]
    [Tooltip("Calculated dps")]
    private float dps;
    [SerializeField]
    [Tooltip("Time between requesting and lauching an attack")]
    private float attackPoint;
    [SerializeField]
    private float attackBackswing;
    [SerializeField]
    [Tooltip("Attack speed")]
    private float attacksPerSecond;
    [SerializeField]
    [Tooltip("Time taken to attack")]
    private float attackTime;
    [SerializeField]
    private int attackRange;
    [SerializeField]
    private float manaPerAttack;
    [SerializeField]
    private int movementSpeed;

    [Header("DEF")]
    [Header("DO NOT ALTER HERE")]
    [Header("CALCULATED STATS")]
    [SerializeField]
    private int health;
    [SerializeField]
    [Tooltip("Health with bonus")]
    private int maxHealth;
    [SerializeField]
    private int mana;
    [SerializeField]
    private int armor;
    [SerializeField]
    private float physDmgReduction;

    [Header("BONUS STATS")]
    [Header("DO NOT ALTER HERE")]
    [Header("CALCULATED STATS")]
    [SerializeField]
    [Tooltip("Bonus armor recieved")]
    private int bonusArmor;
    [SerializeField]
    [Tooltip("Bonus attack speed")]
    private float bonusAttackSpeed;
    [Tooltip("Bonus damage")]
    private int bonusDamage;

    [Header("EXACT FROM SYNERGIES BONUS STATS")]
    [Header("DO NOT ALTER HERE")]
    [Header("CALCULATED STATS")]
    [SerializeField]
    [Tooltip("Bonus armor recieved")]
    private int synergies_bonusArmor;
    [SerializeField]
    [Tooltip("Bonus attack speed")]
    private float synergies_bonusAttackSpeed;
    [Tooltip("Bonus damage")]
    private int synergies_bonusDamage;

    public UnitStats Stats { get => stats; protected set => stats = value; }

    public int MinAttackDmg { get => minAttackDamage; protected set => minAttackDamage = value; }
    public int MaxAttackDmg { get => maxAttackDamage; protected set => maxAttackDamage = value; }
    public float Dps { get => dps; protected set => dps = value; }
    public float AttackPoint { get => attackPoint; protected set => attackPoint = value; }
    public float AttackBackswing { get => attackBackswing; protected set => attackBackswing = value; }
    public float AttacksPerSecond { get => attacksPerSecond; protected set => attacksPerSecond = value; }
    public float AttackTime { get => attackTime; protected set => attackTime = value; }
    public int AttackRange { get => attackRange; protected set => attackRange = value; }

    public int Health { get => health; protected set => health = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int Mana { get => mana; protected set => mana = value; }
    public int Armor { get => armor; protected set => armor = value; }
    public float PhysDmgReduction { get => physDmgReduction; protected set => physDmgReduction = value; }
    public float ManaPerAttack { get => manaPerAttack; protected set => manaPerAttack = value; }
    public int MoveSpeed { get => movementSpeed; protected set => movementSpeed = value; }

    // bonus stats
    public float BonusAttackSpeed { get => bonusAttackSpeed; set => bonusAttackSpeed = value; }
    public int BonusArmor { get => bonusArmor; set => bonusArmor = value; }
    public int BonusDamage { get => bonusDamage; set => bonusDamage = value; }

    // exact synergy stats

    public float Synergy_BonusAttackSpeed { get => synergies_bonusAttackSpeed; set => synergies_bonusAttackSpeed = value; }
    public int Synergy_BonusArmor { get => synergies_bonusArmor; set => synergies_bonusArmor = value; }
    public int Synergy_BonusDamage { get => synergies_bonusDamage; set => synergies_bonusDamage = value; }

    protected virtual void Awake()
    {
        CalculateAllStats();
    }

    public virtual void ClearSynergyBonus()
    {
        BonusAttackSpeed -= Synergy_BonusAttackSpeed;
        BonusArmor -= Synergy_BonusArmor;

        Synergy_BonusArmor = 0;
        Synergy_BonusAttackSpeed = 0;

        CalculateAllStats();
    }

    public virtual void CalculateAllStats()
    {
        CalculateDPS();
        CalculateAttackSpeed();
        CalculateDamage();
        CalculateAttackRange();
        CalculateMovSpeed();
        CalculateHealth();
        CalculateMana();
        CalculatePhysicalReduction();
    }

    protected virtual void CalculateDPS()
    {
        CalculateAttackSpeed();
        CalculateDamage();

        Dps = (MaxAttackDmg - ((MaxAttackDmg - MinAttackDmg) / 2)) * AttacksPerSecond;
    }

    protected virtual void CalculateAttackSpeed()
    {
        AttacksPerSecond = ((100 + BonusAttackSpeed) * 0.01f) / Stats.baseAttackTime;

        AttackTime = 1 / AttacksPerSecond;

        AttackPoint = Stats.attackPoint / (1 + BonusAttackSpeed);
    }
    protected virtual void CalculateDamage()
    {
        MinAttackDmg = Stats.minAttackDamage;
        MaxAttackDmg = Stats.maxAttackDamage;
    }
    protected virtual void CalculateAttackRange()
    {
        AttackRange = Stats.attackRange;
    }
    protected virtual void CalculateManaPerAttack()
    {
        ManaPerAttack = Stats.manaGeneratedPerAttack;
    }
    protected virtual void CalculateMovSpeed()
    {
        MoveSpeed = stats.movementSpeed;
    }
    protected virtual void CalculateHealth()
    {
        Health = Stats.health;
    }
    protected virtual void CalculateMana()
    {
        Mana = Stats.mana;
    }
    protected virtual void CalculatePhysicalReduction()
    {
        Armor = Stats.armor;

        PhysDmgReduction = (Armor + BonusArmor) * 1.7f;
    }



}

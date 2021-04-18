using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Stat", menuName = "Custom/UnitStats")]
public class UnitStats : ScriptableObject
{
    [Header("General")]
    public new string name;
    public Sprite icon;
    public GameObject unit;
    public StarRating starRating;
    public UnitQuality unitQuality;
    public UnitStats upgradedUnit;

    [Header("Trait")]
    public List<Trait> traits;
    [Header("Class")]
    public List<Class> classes;

    [Header("ATK")]
    [Header("COMBAT STATS")]
    [Tooltip("Only set this if you want the pawn to spawn a projectile on attack (i.e. Ranged pawn)")]
    //---------------------------------
    public GameObject projectilePrefab;
    //---------------------------------
    //every unit
    public int minAttackDamage;
    public int maxAttackDamage;
    public float baseAttackTime;
    public float attackPoint;
    public int attackRange;
    public float manaGeneratedPerAttack;
    public int movementSpeed;

    [Header("DEF")]
    public int health;
    public int mana;
    public int armor;

    // 6 units in each trait
    public enum Trait
    {
        Infernal, //attacks burn targets
        Frostblood, //Thorns and increased defence
        Toxic, //Attacks inflict greavous wounds, toxic units explode on death
        Beast, //killing units restores health and increases attack stats
        Shocktouch //attacks chain to nearby enemies, abillities have a chance to stun
    }
    /// <summary>
    /// assasin 4 /jumps at the start of cb, increased crit% and critdmg
    /// wizzard 4 /increased abillity damage
    /// warrior 6 /increased defence
    /// healer 4 /abillites cast heal allies
    /// specialist 2 /cc duration is doubled for this class
    /// summoner 4 /increased summon damage >>> doubled summons
    /// ranger 4 /more attackspeed >>> every 3 attack does bonus damage
    /// curser 2 /decreases enemy abillity damage
    /// </summary>
    public enum Class
    {
        Assassin,
        Wizard,
        Warrior,
        Healer,
        Specialist,
        Summoner,
        Ranger,
        Curser
    }
    public enum StarRating
    {
        One,
        Two,
        Three
    }
    public enum UnitQuality
    { 
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
}

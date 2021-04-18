using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Synergy", menuName = "Custom/Synergy")]
public class Synergy : ScriptableObject
{
    [Header("GENERAL INFO")]
    public new string name;
    public Sprite icon;
    public Color color;

    [Header("Synergy Buff Info")]
    [Tooltip("This is the total number of buffs this particular synergy can provide. Example: For buffs at 2 of a kind, 4 of a kind, 6 of a kind you would set this to 3.")]
    [Range(1, 3)]
    public int totalBuffSize = 1;

    [Tooltip("This is the total number of units required to max out this particular synergy. Example: totalBuffSize = 3 totalSynergySize = 6 means 3 total buffs or every 2 of a kind (6 / 3 = 2) is a new buff.")]
    [Range(1, 9)]
    public int totalSynergySize = 1;

    [Header("Buffs")]
    [Tooltip("Check this if the buffs this synergy provides are a negative affect meant for the enemy pawns.")]
    public bool enemyDebuff;

    [Tooltip("What to display to the player about the first buff from this synergy, i.e. +10 Armor")]
    [TextArea(3, 5)]
    public string buffOneTooltip;

    [TextArea(3, 5)]
    public string buffTwoTooltip;

    [TextArea(3, 5)]
    public string buffThreeTooltip;

    public delegate void Buff(List<GameObject> units);

    public Buff buff1;
    public Buff buff2;
    public Buff buff3;
}

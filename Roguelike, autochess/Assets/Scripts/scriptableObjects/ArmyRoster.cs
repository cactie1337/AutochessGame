using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Roster", menuName = "Custom/Roster")]
public class ArmyRoster : ScriptableObject
{
    public UnitStats[] roster = new UnitStats[32];
}

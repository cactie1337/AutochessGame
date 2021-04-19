using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRosterManager : Singleton<EnemyRosterManager>
{
    [SerializeField]
    private List<ArmyRoster> rosters;
    public List<ArmyRoster> Rosters { get => rosters; protected set => rosters = value; }

    public virtual int TotalRosterCount()
    {
        return Rosters.Count;
    }
}

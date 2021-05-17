using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRosterManager : Singleton<EnemyRosterManager>
{
    [SerializeField]
    private List<ArmyRoster> rosters;
    public List<ArmyRoster> Rosters { get => rosters; protected set => rosters = value; }


    protected virtual void Awake()
    {
        if(Rosters.Count < 1)
        {
            Debug.LogError("No ArmyRoster objects set in the waves list variable in the EnemyWaveManager script located on the " + gameObject.name + " gamobject. " +
                    "Please add some ArmyRoster objects to that script before entering playmode.");
        }
    }
    public virtual int TotalRosterCount()
    {
        return Rosters.Count;
    }
}

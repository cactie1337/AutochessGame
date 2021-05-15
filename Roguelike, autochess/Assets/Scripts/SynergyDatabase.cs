using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynergyDatabase : Singleton<SynergyDatabase>
{
    [SerializeField]
    private List<Synergy> synergies;

    public List<Synergy> Synergies { get => synergies; protected set => synergies = value; }
}

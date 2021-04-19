using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDatabase : Singleton<UnitDatabase>
{
    // Start is called before the first frame update
    [SerializeField]
    private List<UnitStats> units;

    public List<UnitStats> Units { get => units; protected set => units = value; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchManager : Singleton<BenchManager>
{
    [Header("Bench variables")]
    [SerializeField]
    private int benchSlotCount;
    [SerializeField]
    private Transform benchStartLocation;
    [SerializeField]
    private List<BenchBoardTile> benchSlotScripts = new List<BenchBoardTile>();

    [Header("Combining")]
    [SerializeField]
    private int unitsNeededForCombo;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject benchTilePrefab;

    private ArmyManager armyManagerScript;
    private SynergyManager synergyManagerScript;

    public int BenchSlotCount { get => benchSlotCount; protected set => benchSlotCount = value; }
    protected Transform BenchStartLocation { get => benchStartLocation; set => benchStartLocation = value; }
    public List<BenchBoardTile> BenchSlotScripts { get => benchSlotScripts; set => benchSlotScripts = value; }
    public int UnitsNeededForCombo { get => unitsNeededForCombo; protected set => unitsNeededForCombo = value; }
    protected GameObject BenchTilePrefab { get => benchTilePrefab; set => benchTilePrefab = value; }

    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected SynergyManager SynergyManagerScript { get => synergyManagerScript; set => synergyManagerScript = value; }

    public virtual void Awake()
    {
        ArmyManagerScript = ArmyManager.Instance;
        SynergyManagerScript = SynergyManager.Instance;

        if (BenchSlotCount == 0)
            BenchSlotCount = 8;

        if (UnitsNeededForCombo == 0)
            UnitsNeededForCombo = 3;

        CreateBenchTiles();
    }
    protected virtual void CreateBenchTiles()
    {
        for (int i = 0; i < BenchSlotCount; i++)
        {
            GameObject benchTile = Instantiate(BenchTilePrefab, BenchStartLocation);

            benchTile.name = "Bench tile " + i.ToString();

            benchTile.transform.localPosition = new Vector3(i * 5, 0, 0);

            BenchBoardTile tileScript = benchTile.GetComponent<BenchBoardTile>();

            tileScript.Setup(i, new Vector2(i, 0), BoardTile.TileCategory.Bench);

            BenchSlotScripts.Add(tileScript);
        }
    }
    public virtual bool BenchHasSpace()
    {
        bool hasSpace = false;
        for (int i = 0; i < BenchSlotScripts.Count; i++)
        {
            if(!BenchSlotScripts[i].HasActiveUnit())
            {
                hasSpace = true;

                break;
            }
        }
        return hasSpace;
    }
    //public virtual bool AddNewUnitToBench(UnitStats unitStats, int goldCost)
    //{
    //    for (int i = 0; i < BenchSlotScripts.Count; i++)
    //    {
    //        if (!BenchSlotScripts[i].HasActiveUnit())
    //        {
    //            BenchSlotScripts[i].CreatePlayerUnit(unitStats, goldCost);

    //            SynergyManagerScript.UnitAcquired(unitStats);

    //            CheckForCombination(unitStats);

    //            return true;
    //        }
    //    }
    //    return false;
    //}


}

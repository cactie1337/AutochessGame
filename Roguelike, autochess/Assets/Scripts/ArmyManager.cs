using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : Singleton<ArmyManager>
{
    [Header("Player Army")]
    [SerializeField]
    private int maxArmySize;
    [SerializeField]
    private List<GameObject> activePlayerUnits = new List<GameObject>();
    [SerializeField]
    private List<GameObject> playerRoundStartRoster = new List<GameObject>();
    [SerializeField]
    private List<GameObject> playerTotalRoster = new List<GameObject>();

    [Header("Enemy Army")]
    [SerializeField]
    private List<GameObject> activeEnemyUnits = new List<GameObject>();
    [SerializeField]
    private List<GameObject> enemyRoundStartRoster = new List<GameObject>();

    private BoardManager boardManagerScript;
    private GameManager gameManagerScript;
    private UIManager uiManagerScript;
    private BenchManager benchManagerScript;

    public int MaxArmySize { get => maxArmySize; protected set => maxArmySize = value; }
    public List<GameObject> ActivePlayerUnits { get => activePlayerUnits; protected set => activePlayerUnits = value; }
    public List<GameObject> PlayerRoundStartRoster { get => playerRoundStartRoster; protected set => playerRoundStartRoster = value; }
    public List<GameObject> PlayerTotalRoster { get => playerTotalRoster; protected set => playerTotalRoster = value; }
    public List<GameObject> ActiveEnemyUnits { get => activeEnemyUnits; protected set => activeEnemyUnits = value; }
    public List<GameObject> EnemyRoundStartRoster { get => enemyRoundStartRoster; protected set => enemyRoundStartRoster = value; }
    
    protected BoardManager BoardManagerScript { get => boardManagerScript; set => boardManagerScript = value; }
    protected GameManager GameManagerScript { get => gameManagerScript; set => gameManagerScript = value; }
    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }
    protected BenchManager BenchManagerScript { get => benchManagerScript; set => benchManagerScript = value; }

    protected virtual void Awake()
    {
        BoardManagerScript = BoardManager.Instance;
        GameManagerScript = GameManager.Instance;
        BenchManagerScript = BenchManager.Instance;
        UIManagerScript = UIManager.Instance;
        if (!BoardManagerScript)
        {
            Debug.LogError("No ChessBoardManager singleton instance found in the scene. Please add a ChessBoardManager script to the Game Mananger gameobject before" +
                "entering playmode!");
        }
        if (!GameManagerScript)
        {
            Debug.LogError("No GameManager singleton instance found in the scene.Please add a GameManager script to the Game Mananger gameobject before" +
                "entering playmode!");
        }
        if (!BenchManagerScript)
        {
            Debug.LogError("No BenchManager singleton instance found in the scene.Please add a BenchManager script to the Game Mananger gameobject before" +
                "entering playmode!");
        }
        if (!UIManagerScript)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene.Please add a UserInterfaceManager script to the Game Mananger gameobject before" +
                "entering playmode!");
        }



        MaxArmySize = 0;
    }
    public virtual void IncreaseMaxArmySize(int amount)
    {
        MaxArmySize += amount;

        UIManagerScript.UpdateArmyCountDisplay(ActivePlayerUnits.Count, MaxArmySize);
    }

    public virtual void AddUnitToTotalPlayerRoster(GameObject unit)
    {
       PlayerTotalRoster.Add(unit);
    }
    public virtual void RemoveUnitFromTotalPlayerRoster(GameObject unit)
    {
        PlayerTotalRoster.Remove(unit);
    }
    public virtual void RemoveActiveUnitFromPlayerRoster(GameObject unit)
    {
        ActivePlayerUnits.Remove(unit);
        UIManagerScript.UpdateArmyCountDisplay(ActivePlayerUnits.Count, MaxArmySize);
        if(GameManagerScript.InCombat)
        {
            CheckIfEnemyWonRound();
        }
    }
    public virtual void CheckIfEnemyWonRound()
    {
        if (ActivePlayerUnits.Count <= 0)
        {
            //enemy has won :(
            GameManagerScript.EnemyWonRound();
        }
    }
    
    public virtual void RemoveActiveUnitFromEnemyRoster(GameObject unit)
    {
        ActiveEnemyUnits.Remove(unit);
        if(GameManagerScript.InCombat)
        {
            CheckIfPlayerWonRound();
        }
    }
    public virtual void CheckIfPlayerWonRound()
    {
        if (ActiveEnemyUnits.Count <= 0)
        {
            //player has won!
            GameManagerScript.PlayerWonRound();
        }
    }
    public virtual void AddActiveUnitToPlayerRoster(GameObject unit)
    {
        ActivePlayerUnits.Add(unit);
        UIManagerScript.UpdateArmyCountDisplay(ActivePlayerUnits.Count, MaxArmySize);
    }
    public virtual void CheckIfPlayerIsOverMaxArmySize()
    {
        if(ActivePlayerUnits.Count > MaxArmySize)
        {
            int overageAmount = ActivePlayerUnits.Count - MaxArmySize;
            int iteration = ActivePlayerUnits.Count - 1;

            for (int i = iteration; i > (iteration - overageAmount); i--)
            {
                if(BenchManagerScript.SendActiveUnitToBench(ActivePlayerUnits[i]))
                {
                    RemoveActiveUnitFromPlayerRoster(ActivePlayerUnits[i]);
                }
                else
                {
                    GameManagerScript.SellActiveUnit(ActivePlayerUnits[i]);
                }
            }
        }
    }
    public virtual void SetPlayerRoster()
    {
        PlayerRoundStartRoster.Clear();
        foreach(GameObject unit in ActivePlayerUnits)
        {
            PlayerRoundStartRoster.Add(unit);
        }
    }
    public virtual void ChangeActiveEnemyRoster(List<GameObject> units)
    {
        ActiveEnemyUnits.Clear();
        ActiveEnemyUnits = units;
        EnemyRoundStartRoster.Clear();
        foreach (GameObject unit in ActiveEnemyUnits)
        {
            EnemyRoundStartRoster.Add(unit);
        }
    }
    public virtual void CreateEnemyArmy(ArmyRoster armyRoster)
    {
        List<GameObject> army = new List<GameObject>();

        for (int i = 0; i < armyRoster.roster.Length; i++)
        {
            UnitStats curretnUnitStats = armyRoster.roster[i];
            if(curretnUnitStats != null)
            {
                GameObject enemyUnit = Instantiate(curretnUnitStats.unit);
                army.Add(enemyUnit);
                int enemyTileId = i + 32;
                BoardManagerScript.BoardTiles[enemyTileId].ChangeUnitOutOfCombat(enemyUnit);
            }
        }
        ChangeActiveEnemyRoster(army);
    }
    public virtual void EndCombatForAllActiveUnits()
    {
        foreach (GameObject unit in EnemyRoundStartRoster)
        {
            unit.GetComponent<Status>().EndCombat();
        }

        foreach(GameObject unit in PlayerRoundStartRoster)
        {
            unit.GetComponent<Status>().EndCombat();
            unit.GetComponent<Unit>().ClearSynergyBonus();
        }
    }
    public virtual void DestroyAndResetEnemyRoster()
    {
        ActiveEnemyUnits.Clear();
        for (int i = EnemyRoundStartRoster.Count - 1; i >= 0; i--)
        {
            EnemyRoundStartRoster[i].GetComponent<Status>().SelfDestruct();
        }
        EnemyRoundStartRoster.Clear();
    }
    public virtual void ResetActivePlayerUnits()
    {
        ActivePlayerUnits.Clear();

        foreach (GameObject unit in PlayerRoundStartRoster)
        {
            unit.GetComponent<Status>().ResetUnitAfterCombat();
        }

        PlayerRoundStartRoster.Clear();
    }

    public virtual GameObject SearchForEnemyTarget(Vector3 myPosiotion)
    {
        GameObject target = null;

        float nearestTargetDistance = 999;

        foreach(GameObject enemyUnit in ActiveEnemyUnits)
        {
            float distance = Vector3.Distance(myPosiotion, enemyUnit.transform.position);

            if(distance < nearestTargetDistance)
            {
                target = enemyUnit;
                nearestTargetDistance = distance;
            }
        }
        return target;
    }

    public virtual GameObject SearchForPlayerTarget(Vector3 myPosiotion)
    {
        GameObject target = null;

        float nearestTargetDistance = 999;

        foreach (GameObject playerUnit in ActivePlayerUnits)
        {
            float distance = Vector3.Distance(myPosiotion, playerUnit.transform.position);

            if (distance < nearestTargetDistance)
            {
                target = playerUnit;
                nearestTargetDistance = distance;
            }
        }
        return target;
    }


}

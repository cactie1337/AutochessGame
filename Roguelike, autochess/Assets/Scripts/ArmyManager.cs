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
        MaxArmySize = 0;
    }
    public virtual void IncreaseMaxArmySize(int amount)
    {
        MaxArmySize += amount;

        //UIManagerScript.UpdateArmyCountDisplay(ActivePlayerUnits.Count, MaxArmySize);
    }

    public virtual void AddUnitToTotalPlayerRoster(GameObject unit)
    {
       PlayerTotalRoster.Add(unit);
    }
    public virtual void RemoveUnitFromPlayerRoster(GameObject unit)
    {
        PlayerTotalRoster.Remove(unit);
    }
    public virtual void RemoveActiveUnitFromPlayerRoster(GameObject unit)
    {
        ActivePlayerUnits.Remove(unit);

        //UIManagerScript.UpdateArmyCountDisplay(ActivePlayerUnit.Count, MaxArmySize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Global Variables")]
    [SerializeField]
    private bool inCombat;
    [SerializeField]
    private int currentRound;
    [SerializeField]
    private int rerollCost;
    [SerializeField]
    private int startingGold;

    [Header("Round Specific Variables")]
    [SerializeField]
    private bool roundIsEnding;
    [SerializeField]
    [Tooltip("This is the amount of time the game will wait at the first EndRound call before deciding a winner. Set to 1 by default if 0 at runtime.")]
    private float endRoundLeewayDuration;
    [SerializeField]
    private bool playerArmyWon;
    [SerializeField]
    private bool enemyArmyWon;

    private UIManager uiManagerScript;
    private UnitDatabase unitDatabaseScript;
    private ArmyManager armyManagerScript;
    private EnemyRosterManager enemyRosterManagerScript;
    private UnitDragManager unitDragScript;
    private GoldManager goldManagerScript;
    private ExperienceManager expManager;
    private SynergyManager synergyManagerScript;

    /// <summary>
    /// ----test----
    /// </summary>
    private BenchManager benchManager;

    public bool InCombat { get => inCombat; protected set => inCombat = value; }
    public int CurrentRound { get => currentRound; set => currentRound = value; }
    protected int RerollCost { get => rerollCost; set => rerollCost = value; }
    public int StartingGold { get => startingGold; set => startingGold = value; }
    protected bool RoundIsEnding { get => roundIsEnding; set => roundIsEnding = value; }
    protected float EndRoundLeewayDuration { get => endRoundLeewayDuration; set => endRoundLeewayDuration = value; }
    protected bool PlayerArmyWon { get => playerArmyWon; set => playerArmyWon = value; }
    protected bool EnemyArmyWon { get => enemyArmyWon; set => enemyArmyWon = value; }

    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }
    protected UnitDatabase UnitDatabaseScript { get => unitDatabaseScript; set => unitDatabaseScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected EnemyRosterManager EnemyRosterManagerScript { get => enemyRosterManagerScript; set => enemyRosterManagerScript = value; }
    protected UnitDragManager UnitDragScript { get => unitDragScript; set => unitDragScript = value; }
    protected GoldManager GoldManagerScript { get => goldManagerScript; set => goldManagerScript = value; }
    protected ExperienceManager ExpManager { get => expManager; set => expManager = value; }
    protected SynergyManager SynergyManagerScript { get => synergyManagerScript; set => synergyManagerScript = value; }
    protected BenchManager BenchManagerScript { get => benchManager; set => benchManager = value; }


    protected virtual void Awake()
    {
        //Initialize references
        UIManagerScript = UIManager.Instance;
        SynergyManagerScript = SynergyManager.Instance;
        UnitDatabaseScript = UnitDatabase.Instance;
        ArmyManagerScript = ArmyManager.Instance;
        EnemyRosterManagerScript = EnemyRosterManager.Instance;
        BenchManagerScript = BenchManager.Instance;
        //UnitDragScript = UnitDragManager.Instance;
        //GoldManagerScript = GoldManager.Instance;
        //ExpManager = ExperienceManager.Instance;

        //if we didnt set a leeyway duration in the inspector, set it to 1
        if (EndRoundLeewayDuration == 0)
        {
            EndRoundLeewayDuration = 1f;
        }

        //set to 2 by default if 0 at runtime
        if (RerollCost == 0)
        {
            RerollCost = 2;
        }

        //make sure we dont accidently start the player with no money
        if (StartingGold == 0)
        {
            StartingGold = 1;
        }

        //Setup Scene
        CurrentRound = 0;

        //UI setup
        //UIManager.CreateShopSlots();
    }
    protected virtual void Start()
    {
        BeginGame();
    }
    protected virtual void IncreaseRoundCounter()
    {
        CurrentRound++;
        //UI
    }
    protected virtual void BeginGame()
    {
        IncreaseRoundCounter();
    }
}

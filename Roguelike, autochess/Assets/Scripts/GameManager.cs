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

    private UIManager uiManager;
    private UnitDatabase unitDatabaseScript;
    private ArmyManager armyManagerScript;
    private EnemyRosterManager enemyRosterManagerScript;
    private UnitDragManager unitDragScript;
    private GoldManager goldManagerScript;
    private ExperienceManager expManager;
    private SynergyManager synergyManagerScript;

    public bool InCombat { get => inCombat; protected set => inCombat = value; }
    public int CurrentRound { get => currentRound; set => currentRound = value; }
    protected int RerollCost { get => rerollCost; set => rerollCost = value; }
    public int StartingGold { get => startingGold; set => startingGold = value; }
    protected bool RoundIsEnding { get => roundIsEnding; set => roundIsEnding = value; }
    protected float EndRoundLeewayDuration { get => endRoundLeewayDuration; set => endRoundLeewayDuration = value; }
    protected bool PlayerArmyWon { get => playerArmyWon; set => playerArmyWon = value; }
    protected bool EnemyArmyWon { get => enemyArmyWon; set => enemyArmyWon = value; }

    protected UIManager UIManager { get => uiManager; set => uiManager = value; }
    protected UnitDatabase UnitDatabaseScript { get => unitDatabaseScript; set => unitDatabaseScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected EnemyRosterManager EnemyRosterManagerScript { get => enemyRosterManagerScript; set => enemyRosterManagerScript = value; }
    protected UnitDragManager UnitDragScript { get => unitDragScript; set => unitDragScript = value; }
    protected GoldManager GoldManagerScript { get => goldManagerScript; set => goldManagerScript = value; }
    protected ExperienceManager ExpManager { get => expManager; set => expManager = value; }
    protected SynergyManager SynergyManagerScript { get => synergyManagerScript; set => synergyManagerScript = value; }
}

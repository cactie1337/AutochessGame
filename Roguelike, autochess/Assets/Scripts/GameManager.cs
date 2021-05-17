using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        UnitDragScript = UnitDragManager.Instance;
        GoldManagerScript = GoldManager.Instance;
        ExpManager = ExperienceManager.Instance;
        if (!UIManagerScript)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene. Please add a UserInterfaceManager script to the Game Manager gameobject before" +
                "entering playmode!");
        }

        if (!SynergyManagerScript)
        {
            Debug.LogError("No SynergyManager singleton instance found in the scene. Please add a SynergyManager script to the Game Manager gameobject before" +
                "entering playmode!");
        }

        if (!UnitDatabaseScript)
        {
            Debug.LogError("No PawnDatabase singleton instance found in the scene. Please add a PawnDatabase script to the Database gameobject before" +
                  "entering playmode!");
        }

        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager singleton instance found in the scene. Please add a ArmyManager script to the Game Manager gameobject before" +
                  "entering playmode!");
        }

        if (!EnemyRosterManagerScript)
        {
            Debug.LogError("No EnemyRosterManager singleton instance found in the scene. Please add a EnemyRosterManager script to the Game Manager gameobject before" +
                  "entering playmode!");
        }

        if (!UnitDragScript)
        {
            Debug.LogError("No PawnDragManager singleton instance found in the scene. Pleas add a PawnDragManager script to the Game Manager gameobject before" +
                "entering playmode.");
        }

        if (!GoldManagerScript)
        {
            Debug.LogError("No GoldManager singletone instance found! Please add a GoldManager script to the scene.");
        }

        if (!ExpManager)
        {
            Debug.LogError("No ExperienceManager singletone instance found! Please add a ExperienceManager script to the scene.");
        }

        //if we didnt set a leeyway duration in the inspector, set it to 1
        if (EndRoundLeewayDuration == 0)
        {
            EndRoundLeewayDuration = 1f;
        }

        //set to 2 by default if 0 at runtime
        if (RerollCost == 0)
        {
            RerollCost = 0;
        }

        //make sure we dont accidently start the player with no money
        if (StartingGold == 0)
        {
            StartingGold = 100000;
        }

        //Setup Scene
        CurrentRound = 0;

        //UI setup
        UIManagerScript.CreateShopSlots();
    }
    protected virtual void Start()
    {
        BeginGame();
    }
    protected virtual void IncreaseRoundCounter()
    {
        CurrentRound++;
        UIManagerScript.UpdateCurrentRoundText(CurrentRound);

    }
    public virtual void BeginGame()
    {
        RerollShopAtEndofRound();
        IncreaseRoundCounter();
        GoldManagerScript.GainGold(StartingGold);
        GrantEndOfRoundExp();
    }
    public virtual void BeginRound()
    {
        if (InCombat)
            return;


        UIManagerScript.ArmyCountDisplay.Close();
        UIManagerScript.CloseShopMenu();

        if (UnitDragScript.IsClickedOnUnit())
        {
            UnitDragScript.ClearClickedUnit();
        }

        if (UnitDragScript.IsDraggingUnit())
        {
            UnitDragScript.SendUnitBack();
        }

        if (EnemyRosterManagerScript.TotalRosterCount() < 1)
        {
            Debug.LogError("No rosters set up. Exiting start of combat. Set some rosters in the EnemyRosterManager before starting combat.");
            return;
        }

        if (CurrentRound - 1 >= EnemyRosterManagerScript.TotalRosterCount())
        {
            ArmyManagerScript.CreateEnemyArmy(EnemyRosterManagerScript.Rosters[EnemyRosterManagerScript.TotalRosterCount() - 1]);
        }
        else
        {
            ArmyManagerScript.CreateEnemyArmy(EnemyRosterManagerScript.Rosters[CurrentRound - 1]);
        }

        ArmyManagerScript.CheckIfPlayerIsOverMaxArmySize();
        ArmyManagerScript.SetPlayerRoster();
        SynergyManagerScript.ApplySynergyEffects(ArmyManagerScript.ActivePlayerUnits, ArmyManagerScript.ActiveEnemyUnits);
        SynergyManagerScript.ApplySynergyEffects(ArmyManagerScript.ActiveEnemyUnits, ArmyManagerScript.ActivePlayerUnits);
        foreach (GameObject unit in ArmyManagerScript.ActivePlayerUnits)
        {
            unit.GetComponent<HealthAndMana>().StartOfCombatHealthRefresh();
        }
        foreach (GameObject unit in ArmyManagerScript.ActiveEnemyUnits)
        {
            unit.GetComponent<HealthAndMana>().StartOfCombatHealthRefresh();
        }
        foreach (GameObject unit in ArmyManagerScript.ActivePlayerUnits)
        {
            unit.GetComponent<Status>().BeginCombat();
        }
        foreach (GameObject unit in ArmyManagerScript.ActiveEnemyUnits)
        {
            unit.GetComponent<Status>().BeginCombat();
        }

        InCombat = true;

        ArmyManagerScript.CheckIfEnemyWonRound();
        ArmyManagerScript.CheckIfPlayerWonRound();
    }

    public virtual void PlayerWonRound()
    {
        PlayerArmyWon = true;
        if(!RoundIsEnding)
        {
            RoundIsEnding = true;
            EndRound();
        }
    }
    public virtual void EnemyWonRound()
    {
        EnemyArmyWon = true;
        if(!RoundIsEnding)
        {
            RoundIsEnding = true;
            EndRound();
            SceneManager.LoadScene(0);
        }
    }
    protected virtual void EndRound()
    {
        ArmyManagerScript.EndCombatForAllActiveUnits();
        StartCoroutine(CountdownToDecideWinner());
        StartCoroutine(ResetBoardAndGiveRewards());
    }
    protected virtual IEnumerator CountdownToDecideWinner()
    {
        yield return new WaitForSeconds(EndRoundLeewayDuration);
        DecideWinnerOfRound();
    }
    protected virtual void DecideWinnerOfRound()
    {
        if(PlayerArmyWon && EnemyArmyWon)
        {
            UIManagerScript.UpdateWinnerMessageText("It's a Draw!");
        }
        else if (PlayerArmyWon)
        {
            UIManagerScript.UpdateWinnerMessageText("Player won!");
            GoldManagerScript.GainGold(1);
        }
        else if (EnemyArmyWon)
        {
            UIManagerScript.UpdateWinnerMessageText("Enemy won!");
        }
        else
        {

        }
        UIManagerScript.WinnerMessagePanel.Open();
        RoundIsEnding = false;
        EnemyArmyWon = false;
        PlayerArmyWon = false;
    }
    protected virtual IEnumerator ResetBoardAndGiveRewards()
    {
        yield return new WaitForSeconds(1);
        ResetBoard();
        UIManagerScript.WinnerMessagePanel.Close();
        IncreaseRoundCounter();
        GrantEndOfRoundExp();
        GrantEndOfRoundGold();
        RerollShopAtEndofRound();
    }
    protected virtual void ResetBoard()
    {
        ArmyManagerScript.DestroyAndResetEnemyRoster();
        ArmyManagerScript.ResetActivePlayerUnits();
        UIManagerScript.ArmyCountDisplay.Open();
        InCombat = false;
    }
    protected virtual void GrantEndOfRoundGold()
    {
        int goldGain = CurrentRound;
        if (goldGain >= 10)
            goldGain = 100;

        GoldManagerScript.GainGold(goldGain);
    }
    protected virtual void GrantEndOfRoundExp()
    {
        ExpManager.GainExperience(1);
    }
    public virtual void RerollShopSlots()
    {
        if (GoldManagerScript.SpendGold(RerollCost))
        {
            UnitStats[] newUnits = new UnitStats[UIManagerScript.ShopSlotCount];

            for (int i = 0; i < UIManagerScript.ShopSlotCount; i++)
            {
                int randomIndex = Random.Range(0, UnitDatabaseScript.Units.Count);
                newUnits[i] = UnitDatabaseScript.Units[randomIndex];
            }
            UIManagerScript.DisplayNewShopLineUp(newUnits);
            UIManagerScript.OpenShopMenu();
        }
        else
        {
            print("More gold is required!");
        }
    }
    protected virtual void RerollShopAtEndofRound()
    {
        UnitStats[] newUnits = new UnitStats[UIManagerScript.ShopSlotCount];
        for (int i = 0; i < UIManagerScript.ShopSlotCount; i++)
        {
            int randomIndex = Random.Range(0, UnitDatabaseScript.Units.Count);
            newUnits[i] = UnitDatabaseScript.Units[randomIndex];
        }
        UIManagerScript.DisplayNewShopLineUp(newUnits);
        UIManagerScript.OpenShopMenu();
    }
    public virtual void PurchaseExp()
    {
        if (GoldManagerScript.SpendGold(0))
        {
            ExpManager.GainExperience(4);
        }
        else
        {
            print("More gold is required!");
        }
    }
    public virtual void SellActiveUnit(GameObject unit)
    {
        ArmyManagerScript.RemoveActiveUnitFromPlayerRoster(unit);
        ArmyManagerScript.RemoveUnitFromTotalPlayerRoster(unit);
        Status status = unit.GetComponent<Status>();
        GoldManagerScript.GainGold(status.GoldWorth);
        status.SelfDestruct();
    }
}

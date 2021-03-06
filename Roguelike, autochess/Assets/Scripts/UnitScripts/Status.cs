using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("General Variables")]
    [SerializeField]
    private bool isDead;
    [SerializeField]
    private bool isPlayer;
    [SerializeField]
    private bool inCombat;

    [Header("Destruction Variable")]
    [SerializeField]
    private int goldWorth;

    private Targeting targetingScript;
    private Movement movementScript;
    private HomeBase homeBaseScript;
    private HealthAndMana healthAndManaScript;
    private ArmyManager armyManagerScript;

    public bool IsDead { get => isDead; set => isDead = value; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }
    public bool InCombat { get => inCombat; set => inCombat = value; }
    public int GoldWorth { get => goldWorth; set => goldWorth = value; }

    protected Targeting TargetingScript { get => targetingScript; set => targetingScript = value; }
    protected Movement MovementScript { get => movementScript; set => movementScript = value; }
    protected HomeBase HomeBaseScript { get => homeBaseScript; set => homeBaseScript = value; }
    protected HealthAndMana HealthAndManaScript { get => healthAndManaScript; set => healthAndManaScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }

    public virtual void Awake()
    {
        ArmyManagerScript = ArmyManager.Instance;
        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager singleton instance found in scene. Please add one before entering playmode!");
        }
        TargetingScript = GetComponent<Targeting>();
        if (!TargetingScript)
        {
            Debug.LogError("No Targetings script on pawn prefab: " + gameObject.name + ". Please add a Targeting script to this pawns prefab before entering playmode.");
        }
        MovementScript = GetComponent<Movement>();
        if (!MovementScript)
        {
            Debug.LogError(gameObject.name + " has no MOvement script. please add one to its prefab before entering playmode.");
        }
        HomeBaseScript = GetComponent<HomeBase>();
        if (!HomeBaseScript)
        {
            Debug.LogError(gameObject.name + " has no HomeBase script. please add one to its prefab before entering playmode.");
        }
        HealthAndManaScript = GetComponent<HealthAndMana>();
        if (!HomeBaseScript)
        {
            Debug.LogError(gameObject.name + " has no HealthAndMana script. please add one to its prefab before entering playmode.");
        }

        IsDead = false;
        IsPlayer = false;
        InCombat = false;
    }
    public virtual void BeginCombat()
    {
        InCombat = true;
        MovementScript.ResetPreviousTiles();
        TargetingScript.SearchForNewTarget();
    }
    public virtual void EndCombat()
    {
        InCombat = false;
    }
    public virtual void ResetUnitAfterCombat()
    {
        IsDead = false;
        ArmyManagerScript.AddActiveUnitToPlayerRoster(gameObject);
        HomeBaseScript.SendToHomeBase();
        HealthAndManaScript.ResetHealthAndMana();
        gameObject.SetActive(true);
    }
    public virtual void SelfDestruct()
    {
        if (healthAndManaScript.HealthBarScript)
            Destroy(HealthAndManaScript.HealthBarScript.gameObject);
        MovementScript.CurrentTile.ClearActiveUnit();
        Destroy(gameObject);
    }

}

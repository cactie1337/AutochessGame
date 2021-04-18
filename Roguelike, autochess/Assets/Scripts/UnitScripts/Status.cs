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
        TargetingScript = GetComponent<Targeting>();
        MovementScript = GetComponent<Movement>();
        HomeBaseScript = GetComponent<HomeBase>();
        HealthAndManaScript = GetComponent<HealthAndMana>();

        IsDead = false;
        IsPlayer = false;
        InCombat = false;
    }
}

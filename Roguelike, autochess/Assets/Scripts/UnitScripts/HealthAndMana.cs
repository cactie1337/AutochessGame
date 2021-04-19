using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndMana : MonoBehaviour
{
    [Header("Health Variables")]
    [SerializeField]
    private float currentHealth;

    [Header("Mana Variables")]
    private float currentMana;

    [Header("Health Bar")]
    [SerializeField]
    private float healthBarOffsetZ;
    private GameObject healthBarGO;
    private Transform healthBarTransform;
    private HealthBar healthBarScript;
    private RectTransform greenBarTransform;
    private RectTransform blueBarTransform;
    private float greenBarStartWidth;
    private float blueBarStartWidth;

    private bool healthBarInitialized = false;

    private Unit unitScript;
    private Camera mainCamera;
    private PrefabDatabase prefabDatabaseScript;
    private UIManager UIManagerScript;
    private Status statusScript;
    private AutoAttack attackScript;
    private Movement movementScript;
    private ArmyManager armyManagerScript;

    public float CurrentHealth { get => currentHealth; protected set => currentHealth = value; }

    public float CurrentMana { get => currentMana; protected set => currentMana = value; }

    public Transform HealthBarTransform { get => healthBarTransform; set => healthBarTransform = value; }

    public HealthBar HealthBarScript { get => healthBarScript; set => healthBarScript = value; }

    public float HealthBarOffsetZ { get => healthBarOffsetZ; set => healthBarOffsetZ = value; }

    protected float GreenBarStartWidth { get => greenBarStartWidth; set => greenBarStartWidth = value; }
    protected float BlueBarStartWidth { get => blueBarStartWidth; set => blueBarStartWidth = value; }

    protected RectTransform GreenBarTransform { get => greenBarTransform; set => greenBarTransform = value; }
    protected RectTransform BlueBarTransform { get => blueBarTransform; set => blueBarTransform = value; }

    //references
    protected Unit UnitScript { get => unitScript; set => unitScript = value; }
    protected Camera MainCamera { get => mainCamera; set => mainCamera = value; }
    protected PrefabDatabase PrefabDatabaseScript { get => prefabDatabaseScript; set => prefabDatabaseScript = value; }
    protected UIManager UIManager { get => UIManager; set => UIManager = value; }
    protected Status StatusScript { get => statusScript; set => statusScript = value; }
    protected AutoAttack AttackScript { get => attackScript; set => attackScript = value; }
    protected Movement MovementScript { get => movementScript; set => movementScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }

    protected virtual void Awake()
    {
        UnitScript = GetComponent<Unit>();
        PrefabDatabaseScript = PrefabDatabase.Instance;
        UIManagerScript = UIManager.Instance;
        ArmyManagerScript = ArmyManager.Instance;
        MainCamera = Camera.main;
        StatusScript = GetComponent<Status>();
        AttackScript = GetComponent<AutoAttack>();
        MovementScript = GetComponent<Movement>();
    }
    protected virtual void Start()
    {
        CurrentHealth = UnitScript.Health;

        CurrentMana = 0;

        HealthBarOffsetZ = -1f;

        InitializeHealthBar();

        SetManaBarSize();
    }
    protected virtual void Update()
    {
        SetHealthBarPostionToUnit();
    }

    protected virtual void SetHealthBarPostionToUnit()
    {
        if(HealthBarTransform)
        {
            Vector3 healthBarPos = MainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 0, HealthBarOffsetZ));
            HealthBarTransform.position = healthBarPos;

        }
    }
    protected virtual void SetHealthBarSize()
    {
        if (!healthBarInitialized)
            InitializeHealthBar();

        float percent = CurrentHealth / UnitScript.Health;
        float newSize = percent * GreenBarStartWidth;

        GreenBarTransform.sizeDelta = new Vector2(newSize, GreenBarTransform.rect.height);
    }

    protected virtual void SetManaBarSize()
    {
        if (!healthBarInitialized)
            InitializeHealthBar();

        float percent = CurrentMana / UnitScript.Mana;
        float newSize = percent * BlueBarStartWidth;

        BlueBarTransform.sizeDelta = new Vector2(newSize, BlueBarTransform.rect.height);
    }

    public virtual void StartOfCombatHealthRefresh()
    {
        CurrentHealth = UnitScript.Health;

        SetHealthBarSize();
    }

    public virtual void TakeDamage(float damage)
    {
        if (StatusScript.IsDead)
            return;

        float mitigatedDamage = damage * (UnitScript.PhysDmgReduction / 100);
        float actualDamage = damage - mitigatedDamage;

        CurrentHealth -= actualDamage;

        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;

            Death();
        }

        SetHealthBarSize();
    }
    public virtual void GainMana(float amount)
    {
        CurrentMana += amount;
        if(CurrentMana >= UnitScript.Mana)
        {
            CurrentMana = UnitScript.Mana;
        }
        SetManaBarSize();
    }

    protected virtual void Death()
    {
        StatusScript.IsDead = true;
        AttackScript.StopAutoAttack();
        HealthBarScript.Disable();
        MovementScript.CurrentTile.ClearActiveUnit();

        if(StatusScript.IsPlayer)
        {
            ArmyManagerScript.RemoveActiveUnitFromPlayerRoster(gameObject);
        }
        else
        {
            ArmyManagerScript.RemoveActiveUnitFromEnemyRoster(gameObject);
        }
        gameObject.SetActive(false);
    }
    public virtual void ResetHealthAndMana()
    {
        CurrentHealth = UnitScript.Health;
        CurrentMana = 0;
        SetHealthBarPostionToUnit();
        SetHealthBarSize();
        SetManaBarSize();
        HealthBarScript.Enalble();
    }
    private void InitializeHealthBar()
    {
        if (healthBarInitialized)
            return;

        GameObject healthBarPrefab = null;

        if (UnitScript.Stats.starRating == UnitStats.StarRating.One)
        {
            healthBarPrefab = PrefabDatabaseScript.oneStarHealthBar;
        }
        else if (UnitScript.Stats.starRating == UnitStats.StarRating.Two)
        {
            healthBarPrefab = PrefabDatabaseScript.twoStarHealthBar;
        }
        else if (UnitScript.Stats.starRating == UnitStats.StarRating.Three)
        {
            healthBarPrefab = PrefabDatabaseScript.threeStarHealthBar;
        }
        

        if(healthBarPrefab != null)
        {
            GameObject healthBar = Instantiate(healthBarPrefab, UIManagerScript.UnitHealthBarCanvas);
            HealthBarTransform = healthBar.transform;
            SetHealthBarPostionToUnit();
            HealthBarScript = healthBar.GetComponent<HealthBar>();

            GreenBarTransform = healthBar.transform.GetChild(0).GetComponent<RectTransform>();
            BlueBarTransform = healthBar.transform.GetChild(1).GetComponent<RectTransform>();

            GreenBarStartWidth = GreenBarTransform.rect.width;
            BlueBarStartWidth = BlueBarTransform.rect.width;

            if(StatusScript.IsPlayer)
            {
                greenBarTransform.GetComponent<Image>().color = Color.green;
            }
            healthBarInitialized = true;
        }
    }

    
      
}

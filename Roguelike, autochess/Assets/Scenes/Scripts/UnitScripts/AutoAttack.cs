using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private bool usingAnimations = false;
    [SerializeField]
    private string attackTriggerString = "Attack 1";


    [Header("Variables")]
    [SerializeField]
    private bool attacking;
    [SerializeField]
    private bool readyToAttack;

    [Header("Projectiles")]
    [SerializeField]
    private Transform projectileSpawnTransform;

    private Targeting targetingScript;
    private Unit unitScript;
    private Movement movementScript;
    private Animator anim;
    private Status statusScript;
    private HealthAndMana healthAndManaScript;

    protected bool UsingAnimations { get => usingAnimations; set => usingAnimations = value; }
    protected string AttackTriggerString { get => attackTriggerString; set => attackTriggerString = value; }
    protected bool Attacking { get => attacking; set => attacking = value; }
    public bool ReadyToAttack { get => readyToAttack; set => readyToAttack = value; }
    protected Transform ProjectileSpawnTransform { get => projectileSpawnTransform; set => projectileSpawnTransform = value; }

    protected Targeting TargetingScript { get => targetingScript; set => targetingScript = value; }
    protected Unit UnitScript { get => unitScript; set => unitScript = value; }
    protected Movement MovementScript { get => movementScript; set => movementScript = value; }
    protected Animator Anim { get => anim; set => anim = value; }
    protected Status StatusScript { get => statusScript; set => statusScript = value; }
    protected HealthAndMana HealthAndManaScript { get => healthAndManaScript; set => healthAndManaScript = value; }

    protected virtual void Awake()
    {
        TargetingScript = GetComponent<Targeting>();
        UnitScript = GetComponent<Unit>();
        MovementScript = GetComponent<Movement>();
        StatusScript = GetComponent<Status>();
        HealthAndManaScript = GetComponent<HealthAndMana>();

        if (usingAnimations)
        {
            Anim = GetComponent<Animator>();
            if (!Anim)
            {
                Debug.LogError("AutoAttack script has 'useAnimations' set to true but does not have an Animator component.");
            }

            if (AttackTriggerString == "")
            {
                Debug.LogWarning("No 'AttackTriggerString' set in the AutoAttack script for the " + gameObject.name + " pawn. Please set the name of the trigger " +
                    "in the corresponding animator componenet or disable animations on the prefab");
            }
        }
    }
    protected virtual void Update()
    {
        if (StatusScript.IsDead)
            return;
        if(ReadyToAttack && !Attacking)
        {
            if(TargetingScript.TargetsUnitScript.AttackRange < UnitScript.AttackRange)
            {
                BeginAutoAttack();
            }
            else
            {
                if (TargetingScript.TargetsAttackScript.ReadyToAttack)
                {
                    BeginAutoAttack();
                }
            }
        }
    }

    public virtual void BeginAutoAttack()
    {
        Attacking = true;
        StartCoroutine(AutoAttackTimer());
    }
    public virtual void StopAutoAttack()
    {
        Attacking = false;
        ReadyToAttack = false;
        StopAllCoroutines();
    }
    protected virtual IEnumerator AutoAttackTimer()
    {
        if (TargetingScript.TargetIsInRange())
        {
            //start attack point timer
            StartCoroutine(LaunchAttackTimer());

            MovementScript.RotateUnit(TargetingScript.Target.transform);

            //start attack animation if we are using animations
            if (usingAnimations)
                Anim.SetTrigger("Attack 1");

            yield return new WaitForSeconds(UnitScript.AttackTime);

            //restart auto attack timer
            StartCoroutine(AutoAttackTimer());
        }
        else
        {
            StopAutoAttack();

            //we lost range on our target so run the search again
            //a new target may be closer now
            TargetingScript.SearchForNewTarget();
        }
    }
    public virtual IEnumerator LaunchAttackTimer()
    {
        yield return new WaitForSeconds(UnitScript.AttackPoint);
        LauchAttack();
    }

    protected virtual void LauchAttack()
    {
        GameObject projectilePrefab = UnitScript.Stats.projectilePrefab;

        if(projectilePrefab != null)
        {
            GameObject projectile;

            if (ProjectileSpawnTransform != null)
            {
                projectile = Instantiate(projectilePrefab, ProjectileSpawnTransform.position, Quaternion.identity);
            }
            else
            {
                projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            }
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            projectileScript.Setup(TargetingScript.Target.transform, TargetingScript.TargetHealthScript, CalculateDamage());
        }
        else
        {
            TargetingScript.TargetHealthScript.TakeDamage(CalculateDamage());
        }

        HealthAndManaScript.GainMana(UnitScript.ManaPerAttack);
    }




    protected virtual float CalculateDamage()
    {
        float damage = Random.Range(UnitScript.MinAttackDmg, UnitScript.MaxAttackDmg + 1);  

        return damage;
    }

}

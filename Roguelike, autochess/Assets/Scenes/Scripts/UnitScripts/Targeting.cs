using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    private GameObject previousTarget;
    private AutoAttack targetAttackScript;
    private Status targetStatus;
    private HealthAndMana targetHealthScript;
    private Movement targetsMovementScript;
    private Unit targetsUnitScript;

    private Status statusScript;
    private ArmyManager armyManagerScript;
    private Movement movementScript;
    private Unit unitScript;
    private AutoAttack autoAttackScript;

    public GameObject Target { get => target; protected set => target = value; }
    protected GameObject PreviousTarget { get => previousTarget; set => previousTarget = value; }
    public AutoAttack TargetsAttackScript { get => targetAttackScript; protected set => targetAttackScript = value; }
    public Status TargetStatus { get => targetStatus; protected set => targetStatus = value; }
    public HealthAndMana TargetHealthScript { get => targetHealthScript; set => targetHealthScript = value; }
    public Movement TargetsMovementScript { get => targetsMovementScript; set => targetsMovementScript = value; }
    public Unit TargetsUnitScript { get => targetsUnitScript; set => targetsUnitScript = value; }

    protected Status StatusScript { get => statusScript; set => statusScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected Movement MovementScript { get => movementScript; set => movementScript = value; }
    protected Unit UnitScript { get => unitScript; set => unitScript = value; }
    protected AutoAttack AutoAttackScript { get => autoAttackScript; set => autoAttackScript = value; }

    protected virtual void Awake()
    {
        StatusScript = GetComponent<Status>();
        MovementScript = GetComponent<Movement>();
        UnitScript = GetComponent<Unit>();
        AutoAttackScript = GetComponent<AutoAttack>();
        ArmyManagerScript = ArmyManager.Instance;
    }
    public virtual void SearchForNewTarget()
    {
        GameObject newTarget = null;

        if(StatusScript.IsPlayer)
        {
            newTarget = ArmyManagerScript.SearchForEnemyTarget(transform.position);
        }
        else
        {
            newTarget = ArmyManagerScript.SearchForPlayerTarget(transform.position);
        }

        if(newTarget==null)
        {
            return;
        }
        else
        {
            PreviousTarget = Target;

            Target = newTarget;

            TargetsAttackScript = Target.GetComponent<AutoAttack>();
            TargetStatus = Target.GetComponent<Status>();
            TargetHealthScript = Target.GetComponent<HealthAndMana>();
            TargetsMovementScript = Target.GetComponent<Movement>();
            TargetsUnitScript = Target.GetComponent<Unit>();

            RangeCheck();
        }
    }
    public virtual void DelayedSearchForNewTarget(float timer)
    {
        Invoke("SearchForNewTarget", timer);
    }
    public virtual void RangeCheck()
    {
        if (TargetStatus.IsDead)
        {
            SearchForNewTarget();
        }

        if (TargetIsInRange())
        {
            AutoAttackScript.ReadyToAttack = true;
        }
        else
        {
            MovementScript.MoveOneTileToTarget();
        }
    }

    public virtual bool TargetIsInRange()
    {
        if (Target == null)
            return false;
        if (TargetStatus.IsDead)
            return false;

        int range = UnitScript.AttackRange;

        Vector2 targetPos = Target.GetComponent<Movement>().GridPosition;
        Vector2 myPos = GetComponent<Movement>().GridPosition;

        Vector2 gridDistance = targetPos - myPos;

        if(Mathf.Abs(gridDistance.x) <= range && Mathf.Abs(gridDistance.y) <= range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

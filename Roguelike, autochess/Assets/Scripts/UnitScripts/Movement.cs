using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private bool usingAnimations = false;
    [SerializeField]
    private string movementBoolString = "moving";

    [Header("Positioning")]
    [SerializeField]
    private BoardTile currentTile;

    private BoardTile previousTile1;
    private BoardTile previousTile2;
    private BoardTile previousTile3;

    [SerializeField]
    private Vector2 gridPosition;
    [SerializeField]
    private Vector3 desiredPosition;

    [Header("Variables")]
    private bool moving;

    private Targeting targetingScript;
    private Unit unitScript;
    private BoardManager boardManagerScript;
    private Animator anim;

    public string MovementBoolString { get => movementBoolString; set => movementBoolString = value; }
    protected bool Moving { get => moving; set => moving = value; }
    public BoardTile CurrentTile { get => currentTile; set => currentTile = value; }
    protected BoardTile PreviousTile1 { get => previousTile1; set => previousTile1 = value; }
    protected BoardTile PreviousTile2 { get => previousTile2; set => previousTile2 = value; }
    protected BoardTile PreviousTile3 { get => previousTile3; set => previousTile3 = value; }
    public Vector2 GridPosition { get => gridPosition; set => gridPosition = value; }
    protected Vector3 DesiredPosition { get => desiredPosition; set => desiredPosition = value; }
    protected Targeting TargetingScript { get => targetingScript; set => targetingScript = value; }
    protected Unit UnitScript { get => unitScript; set => unitScript = value; }
    protected BoardManager BoardManagerScript { get => boardManagerScript; set => boardManagerScript = value; }
    protected Animator Anim { get => anim; set => anim = value; }

    protected virtual void Awake()
    {
        BoardManagerScript = BoardManager.Instance;

        TargetingScript = GetComponent<Targeting>();
        UnitScript = GetComponent<Unit>();

        if (usingAnimations)
        {
            Anim = GetComponent<Animator>();
            if (!Anim)
            {
                Debug.LogError("AutoAttack script has 'useAnimations' set to true but does not have an Animator component.");
            }

            if (MovementBoolString == "")
            {
                Debug.LogWarning("No 'MovementBoolString' set in the AutoAttack script for the " + gameObject.name + " pawn. Please set the name of the trigger " +
                    "in the corresponding animator componenet or disable animations on the prefab");
            }
        }
    }
    protected virtual void Update()
    {
        if (Moving)
        {
            float distance = Vector3.Distance(transform.position, DesiredPosition);

            if(distance < 0.2f)
            {
                Moving = false;

                TargetingScript.SearchForNewTarget();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, DesiredPosition, UnitScript.MoveSpeed * Time.deltaTime);
            }
        }
        if (usingAnimations)
        {
            Anim.SetBool("moving", Moving);
        }
    }
    protected virtual void SetCurrentTileOutOfCombat(BoardTile tile)
    {
        if (CurrentTile != null)
        {
            if(CurrentTile.ActiveUnit == gameObject)
            {
                CurrentTile.ClearActiveUnit();
            }
        }

        PreviousTile3 = PreviousTile2;
        PreviousTile2 = PreviousTile1;
        PreviousTile1 = CurrentTile;
        //set previous tile
        CurrentTile = tile;
        //set our grid position
        GridPosition = tile.GridPosition;
        //set desired position
        DesiredPosition = tile.transform.position;
    }
    protected virtual void SetCurrentTileOutInCombat(BoardTile tile)
    {
        if (CurrentTile != null)
        {   
                CurrentTile.ClearActiveUnit();   
        }

        PreviousTile3 = PreviousTile2;
        PreviousTile2 = PreviousTile1;
        PreviousTile1 = CurrentTile;
        //set previous tile
        CurrentTile = tile;
        //set our grid position
        GridPosition = tile.GridPosition;
        //set desired position
        DesiredPosition = tile.transform.position;
    }

}

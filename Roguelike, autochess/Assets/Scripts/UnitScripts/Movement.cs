using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Animations")]
    [SerializeField]
    private bool usingAnimations = false;
    [SerializeField]
    private string movementBoolString = "Moving";

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
        if (!BoardManagerScript)
        {
            Debug.LogError("No ChessBoardManager singleton instance found in the scene. Please add a ChessBoardManager script to the GameManager gameobject before" +
                "entering playmode!");
        }

        //cache references for later use
        TargetingScript = GetComponent<Targeting>();
        UnitScript = GetComponent<Unit>();

        //Only set reference to animator if we are using animations
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
            Anim.SetBool("Moving", Moving);
        }
    }
    public virtual void SetCurrentTileOutOfCombat(BoardTile tile)
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
    public virtual void SetCurrentTileInCombat(BoardTile tile)
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
    protected struct TileMovementData
    {
        public BoardTile tile;
        public float distance;
    }
    public virtual void MoveOneTileToTarget()
    {
        BoardTile tile = GetNearestTileClosestToTarget();

        if (tile != null)
        {
            tile.ChangeUnitInCombat(gameObject);
            Moving = true;
            RotateUnit(tile.transform);
        }
        else
        {

        }
    }
    public virtual BoardTile GetNearestTileClosestToTarget()
    {
        if (TargetingScript.Target == null)
            return null;

        List<TileMovementData> nearbyTiles = new List<TileMovementData>();

        for (int z = -1; z <= 1; z++)
        {
            int gridPositionZ = (int)GridPosition.y + z;
            if (gridPositionZ < 0 || gridPositionZ >= 8)
                continue;
            for (int x = -1; x <= 1; x++)
            {
                int gridPositionX = (int)GridPosition.x + x;
                if (gridPositionX < 0 || gridPositionX >= 8)
                    continue;
                int id = ConvertGridPositionToTileId(new Vector2(gridPositionX, gridPositionZ));

                BoardTile tile = BoardManagerScript.BoardTiles[id];

                Vector2 dir = TargetingScript.TargetsMovementScript.GridPosition - tile.GridPosition;

                float distanceSqrMag = Vector2.SqrMagnitude(dir);

                float distance = Vector3.Distance(TargetingScript.Target.transform.position, tile.transform.position);

                TileMovementData tileData = new TileMovementData();
                tileData.tile = tile;
                tileData.distance = distanceSqrMag;

                if(nearbyTiles.Count == 0)
                {
                    nearbyTiles.Add(tileData);
                }
                else
                {
                    bool inserted = false;

                    for(int i = 0; i < nearbyTiles.Count; i++)
                    {
                        if(tileData.distance < nearbyTiles[i].distance)
                        {
                            nearbyTiles.Insert(i, tileData);
                            inserted = true;
                            break;
                        }
                    }

                    if(!inserted)
                    {
                        nearbyTiles.Add(tileData);
                    }
                }
            }
        }
        for (int i = 0; i < nearbyTiles.Count; i++)
        {
            if(PreviousTile1 != null && PreviousTile2 != null && PreviousTile3 != null)
            {
                if(nearbyTiles[i].tile == PreviousTile1 || nearbyTiles[i].tile == PreviousTile2 || nearbyTiles[i].tile == PreviousTile3)
                {
                    continue;
                }
            }

            if(nearbyTiles[i].tile.ActiveUnit == null)
            {
                return nearbyTiles[i].tile;
            }
        }
        TargetingScript.DelayedSearchForNewTarget(0.5f);

        return null;

    }
    public virtual int ConvertGridPositionToTileId(Vector2 position)
    {
        int id = (int)((position.y * 8) + position.x);
            return id;
    }
    public virtual void RotateUnit(Transform target)
    {
        transform.LookAt(target);
    }
    public virtual void ResetPreviousTiles()
    {
        PreviousTile1 = null;
        PreviousTile2 = null;
        PreviousTile3 = null;
    }
    
}

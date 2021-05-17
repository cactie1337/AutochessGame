using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
   

    //Unit info
    [SerializeField]
    private GameObject activeUnit;

    //Tile info
    [SerializeField]
    private int id;
    [SerializeField]
    private Vector2 gridPosition;
    [SerializeField]
    private TileCategory tileType;
    [SerializeField]
    private float defaultUnitYRot;

    //Visual references
    [SerializeField]
    private GameObject hoverHighlight;
    private UnitDragManager unitDragScript;
    private GameManager gameManagerScript;
    private ArmyManager armyManagerScript;
    private UIManager uiManagerScript;


    public enum TileCategory
    {
        None,
        Player,
        Enemy,
        Bench
    }
    public GameObject ActiveUnit { get => activeUnit; protected set => activeUnit = value; }
    public int Id { get => id; protected set => id = value; }
    public Vector2 GridPosition { get => gridPosition; protected set => gridPosition = value; }
    public TileCategory TileType { get => tileType; protected set => tileType = value; }
    protected float DefaultUnitYRotation { get => defaultUnitYRot; set => defaultUnitYRot = value; }
    protected GameObject HoverHighLight { get => hoverHighlight; set => hoverHighlight = value; }

    protected UnitDragManager UnitDragScript { get => unitDragScript; set => unitDragScript = value; }
    protected GameManager GameManager { get => gameManagerScript; set => gameManagerScript = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }

    protected virtual void Awake()
    {
        if(!HoverHighLight)
        {
            Debug.LogError("No hoverHighlight reference set on the ChessBoardTile script on Chess Board Tile prefab. Please drag 'Highlight' child on" +
                "Chess Board Tile prefab into the ChessBoardTile script in the inspector.");
        }
        UnitDragScript = UnitDragManager.Instance;
        if (!UnitDragScript)
        {
            Debug.LogError("No PawnDragManager singleton instance found in the scene. Please add a PawnDragManager script to the Game Manager gameobject before entering playmode!");
        }
        ArmyManagerScript = ArmyManager.Instance;
        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager singleton instance found in the scene. Please add a ArmyManager script to the Game Manager gameobject before entering playmode!");
        }
        GameManager = GameManager.Instance;
        if (!GameManager)
        {
            Debug.LogError("No GameManager singleton instance found in the scene. Please add an AutoBattles.GameManager script to the game manager gameobject " +
                "before entering playmode!");
        }
        UIManagerScript = UIManager.Instance;
        if (!UIManagerScript)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene. Please add an AutoBattles.GameManager script to the game manager gameobject " +
                "before entering playmode!");
        }
    }


    public virtual void Setup(int id, Vector2 gridPosition, TileCategory tileType)
    {
        Id = id;
        GridPosition = gridPosition;
        TileType = tileType;
        SetDefaultYRotation(tileType);
    }
    private void SetDefaultYRotation(TileCategory tileCategory)
    {
        if(tileCategory == TileCategory.Bench)
        {
            DefaultUnitYRotation = 180f;
        }
        else if (tileCategory == TileCategory.Enemy)
        {
            DefaultUnitYRotation = 180f;
        }
        else if (tileCategory == TileCategory.Player)
        {
            DefaultUnitYRotation = 0f;
        }
    }

    public virtual bool HasActiveUnit()
    {
        if(ActiveUnit != null)
            return true;
            else return false;
    }
    public virtual void ClearActiveUnit()
    {
        ActiveUnit = null;
    }

    public virtual void CreatePlayerUnit(UnitStats unitStats, int goldCost)
    {
        if(ActiveUnit != null)
        {
            Destroy(ActiveUnit);
        }
        ActiveUnit = Instantiate(unitStats.unit);

        Status status = ActiveUnit.GetComponent<Status>();
        status.IsPlayer = true;
        status.GoldWorth = goldCost;
        ArmyManagerScript.AddUnitToTotalPlayerRoster(ActiveUnit);
        ChangeUnitOutOfCombat(ActiveUnit);
    
    }
    public virtual void ChangeUnitOutOfCombat(GameObject newUnit)
    {
        if (newUnit == null)
        {
            ClearActiveUnit();
            return;
        }
        newUnit.GetComponent<HomeBase>().SetHomeBase(this);
        newUnit.GetComponent<Movement>().SetCurrentTileOutOfCombat(this);

        ActiveUnit = newUnit;
        ActiveUnit.transform.position = transform.position;
        ActiveUnit.transform.rotation = Quaternion.Euler(0, DefaultUnitYRotation, 0);

            
    }
    public virtual void ChangeUnitInCombat(GameObject newUnit)
    {
        if(newUnit == null)
        {
            ClearActiveUnit();
            return;
        }

        newUnit.GetComponent<Movement>().SetCurrentTileInCombat(this);
        ActiveUnit = newUnit;
    }


    /// <summary>
    /// mouse functions on tiles
    /// </summary>


    protected virtual void OnMouseEnter()
    {
        HoverHighLight.SetActive(true);
        UnitDragScript.SetHoveredTile(this);
    }
    protected virtual void OnMouseExit()
    {
        HoverHighLight.SetActive(false);
        UnitDragScript.SetHoveredTile(null); 
    }
    protected virtual void OnMouseDown()
    {
        UnitDragScript.ClickUnit(ActiveUnit, this);
    }

}



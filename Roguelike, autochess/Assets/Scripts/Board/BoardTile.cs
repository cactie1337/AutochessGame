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

    private UnitDragScript unitDragS;
    private GameManagerScript gameManagerS;
    private ArmyManagerScript armyManagerS;
    private UIManager uiManagerS;


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



    /// <summary>
    /// mouse functions on tiles
    /// </summary>


    protected virtual void OnMouseEnter()
    {
        HoverHighLight.SetActive(true);
    }
    protected virtual void OnMouseExit()
    {
        HoverHighLight.SetActive(false);
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDragManager : Singleton<UnitDragManager>
{
    [SerializeField]
    private GameObject unitClickedOn;
    [SerializeField]
    private GameObject unitBeingDragged;
    [SerializeField]
    private Transform unitTransform;
    [SerializeField]
    private BoardTile tileClickedOn;
    [SerializeField]
    private BoardTile tileDraggedFrom;
    [SerializeField]
    private BoardTile tileHoveredOver;
    [SerializeField]
    private bool hoveredOnTrash;

    [Header("Mouse pos for Drag")]
    [SerializeField]
    private Vector3 lastMousePos = Vector3.zero;
    [SerializeField]
    private bool isDraggingMouse;
    [SerializeField]
    private bool firstTouchTaken;

    [Header("Var")]
    [SerializeField]
    private float zCameraOffset;

    private Camera myCamera;
    private ArmyManager armyManagerScript;
    private GameManager gameManagerScript;
    private GoldManager goldManagerScript;
    private UIManager uiManagerScript;
    private BenchManager benchManagerScript;
    private StatsPanel statsPanelScript;
    private SynergyManager synergyManagerScript;
  
    protected GameObject UnitClickedOn { get => unitClickedOn; set => unitClickedOn = value; }
    protected GameObject UnitBeingDragged { get => unitBeingDragged; set => unitBeingDragged = value; }
    protected Transform UnitTransform { get => unitTransform; set => unitTransform = value; }
    protected BoardTile TileClickedOn { get => tileClickedOn; set => tileClickedOn = value; }
    protected BoardTile TileDraggedFrom { get => tileDraggedFrom; set => tileDraggedFrom = value; }
    protected BoardTile TileHoveredOver { get => tileHoveredOver; set => tileHoveredOver = value; }
    public bool HoveredOnTrash { get => hoveredOnTrash; set => hoveredOnTrash = value; }
    protected Vector3 LastMousePos { get => lastMousePos; set => lastMousePos = value; }
    protected bool IsDraggingMouse { get => isDraggingMouse; set => isDraggingMouse = value; }
    protected bool FirstTouchTaken { get => firstTouchTaken; set => firstTouchTaken = value; }
    protected float ZCameraOffset { get => zCameraOffset; set => zCameraOffset = value; }

    protected Camera MyCamera { get => myCamera; set => myCamera = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected GameManager GameManagerScript { get => gameManagerScript; set => gameManagerScript = value; }
    protected GoldManager GoldManagerScript { get => goldManagerScript; set => goldManagerScript = value; }
    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }
    protected BenchManager BenchManagerScript { get => benchManagerScript; set => benchManagerScript = value; }
    protected StatsPanel StatsPanelScript { get => statsPanelScript; set => statsPanelScript = value; }
    protected SynergyManager SynergyManagerScript { get => synergyManagerScript; set => synergyManagerScript = value; }

    protected virtual void Awake()
    {
        MyCamera = Camera.main;
        ArmyManagerScript = ArmyManager.Instance;
        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager singleton instance found in the scene. Please add an ArmyManager component to the Game Manager gameobject before entering playmode!");
        }
        SynergyManagerScript = SynergyManager.Instance;
        if (!SynergyManagerScript)
        {
            Debug.LogError("No SynergyManager singleton instance found in the scene. Please add an ArmyManager component to the Game Manager gameobject before entering playmode!");
        }
        GameManagerScript = GameManager.Instance;
        if (!GameManagerScript)
        {
            Debug.LogError("No GameManager singleton instance found in the scene. Please add an GameManager script to the game manager gameobject " +
                "before entering playmode!");
        }
        GoldManagerScript = GoldManager.Instance;
        if (!GoldManagerScript)
        {
            Debug.LogError("No GoldManager singleton instance found in the scene. Please add a GoldManager script to the game manager gameobject " +
                "before entering playmode!");
        }
        UIManagerScript = UIManager.Instance;
        if (!UIManagerScript)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene. Please add a UserInterfaceManager script to the game manager gameobject " +
                "before entering playmode!");
        }
        BenchManagerScript = BenchManager.Instance;
        if (!BenchManagerScript)
        {
            Debug.LogError("No BenchManager singleton instance found in the scene. Please add a BenchManager script to the game manager gameobject " +
                "before entering playmode!");
        }
        StatsPanelScript = StatsPanel.Instance;
        if (!StatsPanelScript)
        {
            Debug.LogError("No StatsPanel singleton instance found in the scene. Please add a BenchManager script to the game manager gameobject " +
                "before entering playmode!");
        }

        if (ZCameraOffset == 0)
        {
            ZCameraOffset = 25f;
        }
    }
    protected virtual void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL

        Vector3 mouseDelta = Input.mousePosition - LastMousePos;
        float magnitude = mouseDelta.sqrMagnitude;

        if (magnitude >= 2)
        {
            IsDraggingMouse = true;
        }
        else
        {
            IsDraggingMouse = false;
        }
        LastMousePos = Input.mousePosition;
#endif
        if (UnitClickedOn != null && UnitBeingDragged == null)
        {
            if(TileClickedOn.TileType == BoardTile.TileCategory.Bench)
            {
                if(IsDraggingMouse)
                {
                    DragUnit(UnitClickedOn, TileClickedOn);
                    ClearClickedUnit();
                    return;
                }
                if(Input.GetMouseButtonUp(0))
                {
                    OpenStatsWindow();
                }
            }
            else if (TileClickedOn.TileType == BoardTile.TileCategory.Player)
            {
                if(IsDraggingMouse)
                {
                    if(!GameManagerScript.InCombat)
                    {
                        DragUnit(unitClickedOn, TileClickedOn);
                        ClearClickedUnit();
                        return;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    OpenStatsWindow();
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    OpenStatsWindow();
                }
            }
        }
        if (UnitBeingDragged != null)
        {
            UIManagerScript.TrashPanel.Open();

            SendUnitToMousePos();

            if (Input.GetMouseButtonUp(0))
            {
                if(HoveredOnTrash)
                {
                    if (TileDraggedFrom.TileType == BoardTile.TileCategory.Player)
                    {
                        SellDraggedUnit(true);
                    }
                    else
                    {
                        SellDraggedUnit(false);
                    }
                }
                else
                {
                    if (TileHoveredOver == null)
                    {
                        SendUnitBack();
                    }
                    else
                    {
                        if (TileHoveredOver.TileType == BoardTile.TileCategory.Enemy)
                        {
                            SendUnitBack();
                        }
                        else
                        {
                            if (TileDraggedFrom.TileType == BoardTile.TileCategory.Bench)
                            {
                                if (TileHoveredOver.TileType == BoardTile.TileCategory.Bench)
                                {
                                    if (TileHoveredOver.Id == TileDraggedFrom.Id)
                                    {
                                        SendUnitBack();
                                    }
                                    else
                                    {
                                        SwapUnits();
                                    }
                                }
                                else if (TileHoveredOver.TileType == BoardTile.TileCategory.Player)
                                {
                                    if (GameManagerScript.InCombat)
                                    {
                                        SendUnitBack();
                                        return;
                                    }

                                    SyncArmyChangesFromBench();
                                    SwapUnits();
                                }
                            }
                            else if (TileDraggedFrom.TileType == BoardTile.TileCategory.Player)
                            {
                                if (TileHoveredOver.TileType == BoardTile.TileCategory.Player)
                                {
                                    if (TileHoveredOver.Id == TileDraggedFrom.Id)
                                    {
                                        SendUnitBack();
                                    }
                                    else
                                    {
                                        SwapUnits();
                                    }
                                }
                                else if (TileHoveredOver.TileType == BoardTile.TileCategory.Bench)
                                {
                                    SyncArmyChangesFromBoard();
                                    SwapUnitWithComboCheck();
                                }
                            }
                        }
                    }
                }
                UIManagerScript.TrashPanel.Close();
            }
        }
    }
    protected virtual void OpenStatsWindow()
    {
        UIManagerScript.StatsPanel.Open();
        StatsPanelScript.DisplayNewUnitStats(UnitClickedOn);
        ClearClickedUnit();
    }
    public virtual void CloseStatsWindow()
    {
        UIManagerScript.StatsPanel.Close();
        StatsPanelScript.StopStatTick();
        ClearClickedUnit();
    }
    public virtual void ClickUnit(GameObject unit, BoardTile tileScript)
    {
        UnitClickedOn = unit;

        TileClickedOn = tileScript;
    }
    public virtual void ClearClickedUnit()
    {
        UnitClickedOn = null;
        TileClickedOn = null;
    }
    public virtual bool IsClickedOnUnit()
    {
        if (UnitClickedOn == null)
            return false;
        else
            return true;
    }
    public virtual void DragUnit(GameObject unit, BoardTile tileScript)
    {
        UnitTransform = unit.transform;
        UnitBeingDragged = unit;
        TileDraggedFrom = tileScript;

        CloseStatsWindow();
    }
    public virtual bool IsDraggingUnit()
    {
        if (UnitBeingDragged == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public virtual void SetHoveredTile(BoardTile tileScript)
    {
        TileHoveredOver = tileScript;
    }
    public virtual void Clear()
    {
        UnitBeingDragged = null;
        UnitTransform = null;
        TileDraggedFrom = null;
    }
    protected virtual void SendUnitToMousePos()
    {
        Vector3 point = new Vector3();

        Vector2 mousePos = Input.mousePosition;

        point = MyCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 25));

        UnitTransform.position = Vector3.Lerp(UnitTransform.position, point, 20 * Time.deltaTime);
    }
    public virtual void SendUnitBack()
    {
        TileDraggedFrom.ChangeUnitOutOfCombat(UnitBeingDragged);
        Clear();
    }
    protected virtual void SwapUnits()
    {
        TileDraggedFrom.ChangeUnitOutOfCombat(TileHoveredOver.ActiveUnit);
        TileHoveredOver.ChangeUnitOutOfCombat(UnitBeingDragged);
        Clear();
    }
    protected virtual void SwapUnitWithComboCheck()
    {
        TileDraggedFrom.ChangeUnitOutOfCombat(TileHoveredOver.ActiveUnit);
        TileHoveredOver.ChangeUnitOutOfCombat(UnitBeingDragged);
        BenchManagerScript.CheckForCombination(UnitBeingDragged.GetComponent<Unit>().Stats);
        Clear();
    }
    protected virtual void SyncArmyChangesFromBench()
    {
        if(TileHoveredOver.HasActiveUnit())
        {
            ArmyManagerScript.RemoveActiveUnitFromPlayerRoster(TileHoveredOver.ActiveUnit);
            //SynergyManagerScript.UnitOutOfPlay(TileHoveredOver.ActiveUnit.GetComponent<Unit>().Stats);
        }
        ArmyManagerScript.AddActiveUnitToPlayerRoster(UnitBeingDragged);
        //SynergyManagerScript.UnitInPlay(UnitBeingDragged.GetComponent<Unit>().Stats);
    }
    protected virtual void SyncArmyChangesFromBoard()
    {
        if(TileHoveredOver.HasActiveUnit())
        {
            ArmyManagerScript.AddActiveUnitToPlayerRoster(TileHoveredOver.ActiveUnit);
            //SynergyManagerScript.UnitInPlay(TileHoveredOver.ActiveUnit.GetComponent<Unit>().Stats);
        }
        ArmyManagerScript.RemoveActiveUnitFromPlayerRoster(UnitBeingDragged);
        //SynergyManagerScript.UnitOutOfPlay(UnitBeingDragged.GetComponent<Unit>().Stats);
    }
    protected virtual void SellDraggedUnit(bool draggedFromBoard)
    {
        if (draggedFromBoard)
        {
            ArmyManagerScript.RemoveActiveUnitFromPlayerRoster(UnitBeingDragged);
        }
        //SynergyManagerScript.UnitSold(UnitBeingDragged.GetComponent<Unit>().Stats, draggedFromBoard);
        ArmyManagerScript.RemoveUnitFromTotalPlayerRoster(UnitBeingDragged);
        Status status = UnitBeingDragged.GetComponent<Status>();
        GoldManagerScript.GainGold(status.GoldWorth);

        status.SelfDestruct();
        Clear();
    }
}

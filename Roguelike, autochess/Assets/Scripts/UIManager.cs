using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuObject
{
    public bool isActive;

    public Transform menuTransform;
    public Transform activePosition;
    public Transform hiddenPosition;

    public void Toggle()
    {
        if (isActive)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
    public void Open()
    {
        //set it to its 'active' position
        menuTransform.position = activePosition.position;

        //set it to active
        isActive = true;
    }

    public void Close()
    {
        //set it to its 'hidden' position
        menuTransform.position = hiddenPosition.position;

        //set it to inactive
        isActive = false;
    }

}


public class UIManager : Singleton<UIManager>
{
    [Header("Shop")]
    [SerializeField]
    [Tooltip("This is the reference that will be instatiated at runtime to represent the buttons in the shop.")]
    private GameObject shopSlotPrefab;
    [SerializeField]
    [Tooltip("This is the reference for the parent transform when we instantiate the shop slot prefabs.")]
    private Transform shopSlotGrid;
    [SerializeField]
    [Tooltip("This is a custom class with a group of variables that makes up the shop window behaviour.")]
    private MenuObject shopMenu;
    [SerializeField]
    [Tooltip("This is the number of shop slots that will be created at runtime. Defaults to 5 at runtime.")]
    private int shopSlotCount;
    [SerializeField]
    [Tooltip("Contains the references to all ShopSlot scripts for our active shop slots at runtime.")]
    private List<ShopSlot> shopSlots = new List<ShopSlot>();

    [Header("Experience Button")]
    [SerializeField]
    private Text currentLevelText;
    [SerializeField]
    private Text currentExpText;

    [Header("Gold")]
    [SerializeField]
    [Tooltip("The reference for where we will display the players current gold. Please drag a text element here before runtime!")]
    private Text currentGoldText;

    [Header("Trash Panel")]
    [SerializeField]
    private MenuObject trashPanel;

    [Header("Stats Panel")]
    [SerializeField]
    private MenuObject statsPanel;

    [Header("Tooltip (Desktop)")]
    [SerializeField]
    private MenuObject tooltipPanelDesktop;
    [SerializeField]
    private Text tooltipName;
    [SerializeField]
    private Text tooltipText;

    [Header("Synergy Panel")]
    [SerializeField]
    [Tooltip("Variable that holds the reference to the synergy UI panel.")]
    private MenuObject synergyPanel;
    [SerializeField]
    [Tooltip("Variable that holds the prefab we instantiate for a new synergy. Displays the synergy bubble prefab and Icon")]
    private GameObject synergyWidgetPrefab;
    [SerializeField]
    [Tooltip("Variable that holds the prefab for the bubbles we spawn in the synergyWidgetPrefab")]
    private GameObject synergyBubblePrefab;

    [Header("Army Count Display")]
    [SerializeField]
    private MenuObject armyCountDisplay;
    [SerializeField]
    private Text armyCountText;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color overArmySizeColor;
    [SerializeField]
    private Color underArmySizeColor;

    [Header("Winner Message")]
    [SerializeField]
    private MenuObject winnerMessagePanel;
    [SerializeField]
    private Text winnerMessageText;

    [Header("Round Display")]
    [SerializeField]
    private Text currentRoundText;

    [Header("Unit HealthBars")]
    [SerializeField]
    private Transform unitHealthBarCanvas;

    protected GameObject ShopSlotPrefab { get => shopSlotPrefab; set => shopSlotPrefab = value; }
    protected Transform ShopSlotGrid { get => shopSlotGrid; set => shopSlotGrid = value; }
    protected MenuObject ShopMenu { get => shopMenu; set => shopMenu = value; }
    public int ShopSlotCount { get => shopSlotCount; set => shopSlotCount = value; }
    protected List<ShopSlot> ShopSlots { get => shopSlots; set => shopSlots = value; }
    protected Text CurrentLevelText { get => currentLevelText; set => currentLevelText = value; }
    protected Text CurrentExpText { get => currentExpText; set => currentExpText = value; }
    protected Text CurrentGoldText { get => currentGoldText; set => currentGoldText = value; }
    public MenuObject TrashPanel { get => trashPanel; set => trashPanel = value; }
    public MenuObject StatsPanel { get => statsPanel; protected set => statsPanel = value; }
    protected MenuObject TooltipPanelDesktop { get => tooltipPanelDesktop; set => tooltipPanelDesktop = value; }
    public Text TooltipName { get => tooltipName; set => tooltipName = value; }
    public Text TooltipText { get => tooltipText; set => tooltipText = value; }
    public MenuObject SynergyPanel { get => synergyPanel; protected set => synergyPanel = value; }
    public GameObject SynergyWidgetPrefab { get => synergyWidgetPrefab; protected set => synergyWidgetPrefab = value; }
    public GameObject SynergyBubblePrefab { get => synergyBubblePrefab; protected set => synergyBubblePrefab = value; }
    public MenuObject ArmyCountDisplay { get => armyCountDisplay; set => armyCountDisplay = value; }
    protected Text ArmyCountText { get => armyCountText; set => armyCountText = value; }
    public MenuObject WinnerMessagePanel { get => winnerMessagePanel; set => winnerMessagePanel = value; }
    protected Text WinnerMessageText { get => winnerMessageText; set => winnerMessageText = value; }
    protected Text CurrentRoundText { get => currentRoundText; set => currentRoundText = value; }
    public Transform UnitHealthBarCanvas { get => unitHealthBarCanvas; protected set => unitHealthBarCanvas = value; }

    protected virtual void Awake()
    {
        ShopSlotCount = 5;
    }
    public virtual void UpdateCurrentGoldText(int amount)
    {
        CurrentGoldText.text = amount.ToString();
    }
    public virtual void CreateShopSlots()
    {
        for (int i = 0; i < ShopSlotCount; i++)
        {
            GameObject shopSlotGameobject = Instantiate(ShopSlotPrefab, ShopSlotGrid);
            shopSlotGameobject.name = "Shop slot " + i.ToString();
            ShopSlot shopSlotScript = shopSlotGameobject.GetComponent<ShopSlot>();
            ShopSlots.Add(shopSlotScript);
        }
    }
    public virtual void DisplayNewShopLineUp(UnitStats[] newUnits)
    {
        for (int i = 0; i < ShopSlotCount; i++)
        {
            ShopSlots[i].Setup(newUnits[i]);
        }
    }
    public virtual void ToggleShopMenu()
    {
        ShopMenu.Toggle();
    }
    public virtual void OpenShopMenu()
    {
        ShopMenu.Open();
    }
    public virtual void CloseShopMenu()
    {
        ShopMenu.Close();
    }
    public virtual void UpdateArmyCountDisplay(int currentArmySize, int maxArmySize)
    {
        ArmyCountText.text = currentArmySize.ToString() + " / " + maxArmySize.ToString() + " Active Units";

        if (currentArmySize > maxArmySize)
            ArmyCountText.color = overArmySizeColor;
        else if (currentArmySize < maxArmySize)
            ArmyCountText.color = underArmySizeColor;
        else
            ArmyCountText.color = normalColor;
    }
    public virtual void UpdateCurrentLevelText(int currentLevel)
    {
        CurrentLevelText.text = "Level " + currentLevel.ToString();
    }
    public virtual void UpdateCurrentExpText(int currentExp, int maxExp)
    {
        CurrentExpText.text = currentExp.ToString() + " / " + maxExp.ToString() + " XP";
    }
    public virtual void UpdateWinnerMessageText(string message)
    {
        WinnerMessageText.text = message;
    }
    public virtual void UpdateCurrentRoundText(int round)
    {
        CurrentRoundText.text = "Round " + round.ToString();
    }
    public virtual void OpenDesktopTooltip(Synergy synergy)
    {
        TooltipName.text = synergy.name;

        //grab for later reference
        int sizeForBuff = synergy.totalSynergySize / synergy.totalBuffSize;

        string tooltipDesc = "";

        if (synergy.buffOneTooltip != "")
        {
            tooltipDesc += sizeForBuff.ToString() + "/" + synergy.totalSynergySize.ToString() + ": ";

            tooltipDesc += synergy.buffOneTooltip + "\n";
        }

        if (synergy.buffTwoTooltip != "")
        {
            int secondBuffSize = sizeForBuff * 2;

            tooltipDesc += "\n" + secondBuffSize.ToString() + "/" + synergy.totalSynergySize.ToString() + ": ";

            tooltipDesc += synergy.buffTwoTooltip + "\n";
        }

        if (synergy.buffThreeTooltip != "")
        {
            int thirdBuffSize = sizeForBuff * 3;

            tooltipDesc += "\n" + thirdBuffSize.ToString() + "/" + synergy.totalSynergySize.ToString() + ": ";

            tooltipDesc += synergy.buffThreeTooltip + "\n";
        }

        TooltipText.text = tooltipDesc;

        TooltipPanelDesktop.Open();
    }
    public virtual void CloseDesktopTooltip()
    {
        TooltipPanelDesktop.Close();
    }
}

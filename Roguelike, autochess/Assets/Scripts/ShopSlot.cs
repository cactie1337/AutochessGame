using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [Header("Unit info")]
    [SerializeField]
    private UnitStats unit;
    private int goldCost;

    [Header("Ui References")]
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text _name;
    [SerializeField]
    private Text traitClass;
    [SerializeField]
    private Text goldCostText;

    private BenchManager benchManagerScript;
    private GoldManager goldManagerScript;
    private Button myButton;

    public UnitStats Unit { get => unit; set => unit = value; }

    //how much gold this particular pawn will cost in the shop
    //determined during setup by the quality
    protected int GoldCost { get => goldCost; set => goldCost = value; }

    //UI References
    protected Image Icon { get => icon; set => icon = value; }
    protected Text Name { get => _name; set => _name = value; }
    protected Text TraitClass { get => traitClass; set => traitClass = value; }
    protected Text GoldCostText { get => goldCostText; set => goldCostText = value; }

    //references
    protected BenchManager BenchManagerScript { get => benchManagerScript; set => benchManagerScript = value; }
    protected GoldManager GoldManagerScript { get => goldManagerScript; set => goldManagerScript = value; }


    protected virtual void Awake()
    {
        BenchManagerScript = BenchManager.Instance;
        GoldManagerScript = GoldManager.Instance;

        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => { OnPurchase(); });
    }

    public virtual void Setup(UnitStats unit)
    {
        Unit = unit;
        Icon.sprite = Unit.icon;
        Name.text = Unit.name;
        Name.color = QualityColor(unit.unitQuality);
        GoldCost = QualityCost(unit.unitQuality);
        GoldCostText.text = GoldCost.ToString();

        string TraitAndClass = "";

        foreach (UnitStats.Trait trait in Unit.traits)
        {
            TraitAndClass += trait.ToString() + " ";
        }
        foreach(UnitStats.Class _class in Unit.classes)
        {
            TraitAndClass += _class.ToString() + " ";
        }

        TraitClass.text = TraitAndClass;
    }
    public virtual void Clear()
    {
        Unit = null;
        Icon.sprite = null;
        Name.text = "";
        TraitClass.text = "";
        GoldCost = 0;
        GoldCostText.text = "";
    }
    public virtual void OnPurchase()
    {
        if (Unit == null)
            return;

        if (BenchManagerScript.BenchHasSpace())
        {
            if(GoldManagerScript.SpendGold(GoldCost))
            {
                if (BenchManagerScript.AddNewUnitToBench(Unit, GoldCost))
                {
                    Clear();
                }
                else
                {
                    print("Bench full!");
                }
            }
            else
            {
                print("Not enough gold!");
            }
        }
        else
        {
            print("Bench full!");
        }
    }
    protected virtual Color QualityColor(UnitStats.UnitQuality quality)
    {
        //set our default color in case the switch statement doesnt hit a result
        Color color = Color.white;

        switch (quality)
        {
            case UnitStats.UnitQuality.Common:
                {
                    //do nothing because we want white,
                    //if you want common to have a different color,
                    //set it here
                    //color = new Color(0, 0, 0);
                    break;
                }
            case UnitStats.UnitQuality.Uncommon:
                {
                    //light green
                    color = new Color(77 / 255F, 255 / 255F, 0);
                    break;
                }
            case UnitStats.UnitQuality.Rare:
                {
                    //blue
                    color = new Color(0, 81 / 255F, 255 / 255F);
                    break;
                }
            case UnitStats.UnitQuality.Epic:
                {
                    //purple
                    color = new Color(191 / 255F, 0, 255 / 255F);
                    break;
                }
            case UnitStats.UnitQuality.Legendary:
                {
                    //orange
                    color = new Color(255 / 255F, 149 / 255F, 0);
                    break;
                }
            default:
                {
                    Debug.LogError("We were passed a quality that has not been defined in the QualityColor method on the ShopSlot script. Please assign a case for the " + quality.ToString() + " quality.");
                    break;
                }
        }

        return color;
    }
    protected virtual int QualityCost(UnitStats.UnitQuality quality)
    {
        //set our default cost to 1 in cast the quality passed into the
        //function doesnt catch any cases on the switch statement
        int cost = 1;

        switch (quality)
        {
            case UnitStats.UnitQuality.Common:
                {
                    //do nothing because we want 1 cost common pawns
                    //if you want to change the base cost of common pawns
                    //do so here
                    //cost = 300;
                    break;
                }
            case UnitStats.UnitQuality.Uncommon:
                {
                    cost = 2;
                    break;
                }
            case UnitStats.UnitQuality.Rare:
                {
                    cost = 3;
                    break;
                }
            case UnitStats.UnitQuality.Epic:
                {
                    cost = 4;
                    break;
                }
            case UnitStats.UnitQuality.Legendary:
                {
                    cost = 5;
                    break;
                }
            default:
                {
                    Debug.LogError("We were passed a quality that has not been defined in the QualityCost method on the ShopSlot script. Please assign a case for the " + quality.ToString() + " quality.");
                    break;
                }
        }

        return cost;
    }
}



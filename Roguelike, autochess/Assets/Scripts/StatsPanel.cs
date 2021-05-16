using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsPanel : Singleton<StatsPanel>
{
    [SerializeField]
    private GameObject unit;
    private Unit unitScript;
    private UnitStats unitStats;
    private HealthAndMana healthAndManaScript;

    [Header("Stat UI References")]
    [SerializeField]
    protected Text unitName;
    [SerializeField]
    protected GameObject oneStarParent;
    [SerializeField]
    protected GameObject twoStarParent;
    [SerializeField]
    protected GameObject threeStarParent;
    [SerializeField]
    protected Image unitIcon;
    [SerializeField]
    protected Text unitHealth;
    [SerializeField]
    protected Text unitArmor;
    [SerializeField]
    protected Text unitDamage;
    [SerializeField]
    protected Text unitAttackSpeed;
    [SerializeField]
    protected Text unitDps;
    [SerializeField]
    protected Text unitMoveSpeed;

    private UnitDragManager unitDragScript;

    protected GameObject Unit { get => unit; set => unit = value; }
    protected UnitStats UnitStats { get => unitStats; set => unitStats = value; }
    protected HealthAndMana HealthAndManaScript { get => healthAndManaScript; set => healthAndManaScript = value; }
    protected Unit UnitScript { get => unitScript; set => unitScript = value; }
    protected UnitDragManager UnitDragScript { get => unitDragScript; set => unitDragScript = value; }

    protected virtual void Awake()
    {
        UnitDragScript = UnitDragManager.Instance;
    }
    IEnumerator StatTick()
    {
        yield return new WaitForSeconds(0.1f);

        RefreshStats();

        StartCoroutine("StatTick");
    }
    public virtual void DisplayNewUnitStats(GameObject unit)
    {
        //set reference to the new pawn who's stats we
        //want to display
        Unit = unit;

        //set reference to the actual pawnstats component
        UnitScript = unit.GetComponent<Unit>();
        UnitStats = UnitScript.Stats;
        HealthAndManaScript = Unit.GetComponent<HealthAndMana>();

        RefreshStats();

        //make sure we start our coroutine
        StartCoroutine("StatTick");
    }
    public virtual void StopStatTick()
    {
        StopCoroutine("StatTick");
    }
    protected virtual void RefreshStats()
    {
        if (Unit != null)
        {
            //refresh the stats

            //name
            unitName.text = UnitStats.name;

            //Star Quality
            if (UnitStats.starRating == UnitStats.StarRating.One)
            {
                oneStarParent.SetActive(true);
                twoStarParent.SetActive(false);
                threeStarParent.SetActive(false);
            }
            else if (UnitStats.starRating == UnitStats.StarRating.Two)
            {
                oneStarParent.SetActive(false);
                twoStarParent.SetActive(true);
                threeStarParent.SetActive(false);
            }
            else if (UnitStats.starRating == UnitStats.StarRating.Three)
            {
                oneStarParent.SetActive(false);
                twoStarParent.SetActive(false);
                threeStarParent.SetActive(true);
            }

            //icon
            unitIcon.sprite = UnitStats.icon;

            //health
            unitHealth.text = HealthAndManaScript.CurrentHealth.ToString("F0") + " / " + unitScript.Health.ToString();

            //armor
            string bonusArmorString = BonusStringFormatting(UnitScript.BonusArmor, false);

            unitArmor.text = UnitScript.Armor.ToString() + bonusArmorString;

            //damage

            unitDamage.text = UnitScript.MinAttackDmg.ToString() + "-" + UnitScript.MaxAttackDmg.ToString();

            //attack speed
            string bonusAttackSpeed = BonusStringFormatting((int)UnitScript.Synergy_BonusAttackSpeed, true);

            unitAttackSpeed.text = UnitScript.AttacksPerSecond.ToString("F2") + bonusAttackSpeed;

            //dps
            unitDps.text = UnitScript.Dps.ToString("F1");

            //move speed
            unitMoveSpeed.text = UnitScript.MoveSpeed.ToString();
        }
        else
        {
            UnitDragScript.CloseStatsWindow();
        }
    }
    protected string BonusStringFormatting(int value, bool isPercent)
    {
        string message = "";

        if (isPercent)
        {
            if (value > 0)
            {
                message = "<color=#00ff00> (+" + value.ToString() + "%)</color>";
            }
            else if (value < 0)
            {
                message = "<color=#ff0000> (-" + value + "%)</color>";
            }
            else
            {
                //this means the bonus stat we provided is zero and we dont need to do anything
            }
        }
        else
        {
            if (value > 0)
            {
                message = "<color=#00ff00> (+" + value + ")</color>";
            }
            else if (value < 0)
            {
                message = "<color=#ff0000> (-" + value + ")</color>";
            }
            else
            {
                //this means the bonus stat we provided is zero and we dont need to do anything
            }
        }


        return message;
    }
}

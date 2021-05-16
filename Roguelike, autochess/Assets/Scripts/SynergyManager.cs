using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SynergyObject
{
    public Synergy synergy;
    public Transform widgetTransform;
    public SynergyWidget widgetScript;
    public int uniqueActiveUnits;
    public int uniqueTotalUnits;
    public List<UniqueUnit> activeUnits = new List<UniqueUnit>();
    public List<UniqueUnit> totalUnits = new List<UniqueUnit>();
    private BenchManager benchManagerScript;

    public SynergyObject(Synergy syn, Transform WidgetTransform, SynergyWidget WidgetScript)
    {
        synergy = syn;
        widgetTransform = WidgetTransform;
        widgetScript = WidgetScript;

        uniqueActiveUnits = 0;
        uniqueTotalUnits = 0;
        benchManagerScript = BenchManager.Instance;
    }
    public void UnitAcquired(string unitName)
    {
        if (totalUnits.Count < 1)
        {
            totalUnits.Add(new UniqueUnit(unitName));
            widgetScript.AddOutline();
            widgetScript.ShowWidget();
        }
        else
        {
            bool duplicateFound = false;
            foreach(UniqueUnit entry in totalUnits)
            {
                if(entry.name == unitName)
                {
                    entry.count++;
                    duplicateFound = true;
                }
            }

            if(!duplicateFound)
            {
                totalUnits.Add(new UniqueUnit(unitName));
                widgetScript.AddOutline();
            }
        }
        uniqueTotalUnits = totalUnits.Count;
    }
    public void PawnLost(string unitName)
    {
        bool removalRequired = false;

        UniqueUnit removalReference = new UniqueUnit("");

        foreach (UniqueUnit entry in totalUnits)
        {
            if (entry.name == unitName)
            {
                entry.count--;

                if (entry.count < 1)
                {

                    removalReference = entry;

                    removalRequired = true;
                }

                break;
            }
        }

        if (removalRequired)
        {
            totalUnits.Remove(removalReference);

            widgetScript.RemoveOutline();

            if (totalUnits.Count == 0)
            {
                widgetScript.HideWidget();
            }
        }

        uniqueTotalUnits = totalUnits.Count;
    }
    public void InPlay(string unitName)
    {
        if (activeUnits.Count < 1)
        {
            activeUnits.Add(new UniqueUnit(unitName));

            widgetScript.AddCenterFill();
        }
        else
        {
            bool duplicateFound = false;

            foreach (UniqueUnit entry in activeUnits)
            {
                if (entry.name == unitName)
                {
                    entry.count++;

                    duplicateFound = true;
                }
            }

            if (!duplicateFound)
            {

                activeUnits.Add(new UniqueUnit(unitName));

                widgetScript.AddCenterFill();
            }
        }

        uniqueActiveUnits = activeUnits.Count;
    }
    public void OutOfPlay(string unitName)
    {
        bool removalRequired = false;

        UniqueUnit removalReference = new UniqueUnit("");

        foreach (UniqueUnit entry in activeUnits)
        {
            if (entry.name == unitName)
            {
                entry.count--;

                if (entry.count < 1)
                {

                    removalReference = entry;

                    removalRequired = true;
                }

                break;
            }
        }

        if (removalRequired)
        {
            activeUnits.Remove(removalReference);

            widgetScript.RemoveCenterFill();
        }

        uniqueActiveUnits = activeUnits.Count;
    }
    public void UnitSold(string unitName, bool isInPlay)
    {

        if (isInPlay)
            OutOfPlay(unitName);

        PawnLost(unitName);
    }
    public void AdjustmentFromUpgrade(string unitName)
    {
        //set this reference if we havent already
        if (!benchManagerScript)
        {
            benchManagerScript = BenchManager.Instance;
            
        }

        //search our list to find our unique pawn entry for this particular pawn
        foreach (UniqueUnit entry in totalUnits)
        {
            if (entry.name == unitName)
            {
                entry.count -= (benchManagerScript.UnitsNeededForCombo);

                break;
            }
        }
        uniqueTotalUnits = totalUnits.Count;
    }
}
[System.Serializable]
public class UniqueUnit
{
    public string name = "";
    public int count = 0;

    public UniqueUnit(string Name)
    {
        name = Name;
        count = 1;
    }
}

[System.Serializable]
public class AffectedSynergy
{
    public Synergy.Buff applyBuffs;
    public Synergy synergy;
    public int uniqueUnits;
    public int uniqueUnitsPerBuff;
    public int buffCounter;
    public List<GameObject> affectedUnits = new List<GameObject>();
    public List<UnitStats> affectedUnitStats = new List<UnitStats>();

    public AffectedSynergy(Synergy synergy, UnitData data)
    {
        this.synergy = synergy;

        this.affectedUnits.Add(data.unit);

        this.affectedUnitStats.Add(data.unitStats);

        uniqueUnitsPerBuff = (int)(synergy.totalSynergySize / synergy.totalBuffSize);

        uniqueUnits = 1;

        buffCounter = 0;

        if (uniqueUnits % uniqueUnitsPerBuff == 0 && buffCounter <= synergy.totalBuffSize)
        {
            AddBuff(buffCounter);
        }
    }
    public bool isUnique(string unitName)
    {
        bool duplicateFound = false;

        foreach (UnitStats unit in affectedUnitStats)
        {
            if (unitName == unit.name)
            {
                duplicateFound = true;

                break;
            }
        }

        return !duplicateFound;
    }
    public void AddNewUnit(UnitData data)
    {
        if (isUnique(data.unitStats.name))
        {
            uniqueUnits++;

            if (uniqueUnits % uniqueUnitsPerBuff == 0 && buffCounter <= synergy.totalBuffSize)
            {
                AddBuff(buffCounter);
            }
        }

        affectedUnits.Add(data.unit);
    }
    public void AddBuff(int iteration)
    {
        if (iteration == 0)
        {
            applyBuffs += synergy.buff1;

            buffCounter++;
        }
        else if (iteration == 1)
        {
            applyBuffs += synergy.buff2;

            buffCounter++;
        }
        else if (iteration == 2)
        {
            applyBuffs += synergy.buff3;

            buffCounter++;
        }
        else
        {
            Debug.LogWarning("AddBuff() was passed an iteration higher than 2, we currently only support 3 total buffs per synergy");
        }
    }
}
public class UnitData
{
    public GameObject unit;
    public UnitStats unitStats;
}
public class SynergyManager : Singleton<SynergyManager>
{

    [SerializeField]
    protected List<SynergyObject> synergies = new List<SynergyObject>();


    private SynergyDatabase synergyDatabaseScript;
    private SynergyBuffsAndDebuffs buffScript;
    private UIManager uiManagerScript;

    protected SynergyDatabase SynergyDatabaseScript { get => synergyDatabaseScript; set => synergyDatabaseScript = value; }
    protected UIManager UIManagerScript { get => uiManagerScript; set => uiManagerScript = value; }
    protected SynergyBuffsAndDebuffs BuffScript { get => buffScript; set => buffScript = value; }

    protected virtual void Awake()
    {
        SynergyDatabaseScript = SynergyDatabase.Instance;
        BuffScript = SynergyBuffsAndDebuffs.Instance;
        UIManagerScript = UIManager.Instance;
        //InitializeSynergies();
    }
    protected virtual void InitializeSynergies()
    {
        foreach (Synergy syn in SynergyDatabaseScript.Synergies)
        {
            GameObject widget = Instantiate(UIManagerScript.SynergyWidgetPrefab, UIManagerScript.SynergyPanel.hiddenPosition);

            widget.name = syn.name + " Synergy Widget";

            SynergyWidget widgetScript = widget.GetComponent<SynergyWidget>();
            if (!widgetScript)
            {
                Debug.LogError("No SynergyWidget script found on the Synergy Widget prefab. Please add one to the prefab in the editor before entering playmode!");
            }
            else
            {
                widgetScript.Setup(syn);

                synergies.Add(new SynergyObject(syn, widget.transform, widgetScript));
            }
        }
    }
    //public virtual void UnitAcquired(UnitStats unit)
    //{
    //    //List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].UnitAcquired(unit.name);
    //    }
    //}
    //public virtual void UnitLost(UnitStats unit)
    //{
    //    List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].PawnLost(unit.name);
    //    }
    //}
    //public virtual void UnitInPlay(UnitStats unit)
    //{
    //    List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].InPlay(unit.name);
    //    }
    //}

    
    //public virtual void UnitOutOfPlay(UnitStats unit)
    //{
    //    List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].OutOfPlay(unit.name);
    //    }
    //}
    //public virtual void UnitSold(UnitStats unit, bool isInPlay)
    //{
    //    List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].UnitSold(unit.name, isInPlay);
    //    }
    //}
    //public virtual void AdjustmentFromUpgrade(UnitStats unit)
    //{
    //    List<string> unitSynergies = SynergyStringsToList(unit);

    //    List<int> affectedSynergies = SynergyIterationsAffected(unitSynergies);

    //    foreach (int iteration in affectedSynergies)
    //    {
    //        synergies[iteration].AdjustmentFromUpgrade(unit.name);
    //    }
    //}
    //public virtual void ApplySynergyEffects(List<GameObject> friendlyArmy, List<GameObject> hostileArmy)
    //{
    //    //clear the old list
    //    //affectedSynergies.Clear();
    //    //create a list of affected synergies
    //    List<AffectedSynergy> affectedSynergies = new List<AffectedSynergy>();

    //    List<UnitData> unitData = new List<UnitData>();

    //    foreach (GameObject unit in friendlyArmy)
    //    {
    //        UnitData data = new UnitData();
    //        data.unit = unit;
    //        data.unitStats = unit.GetComponent<Unit>().Stats;

    //        unitData.Add(data);
    //    }

    //    foreach (UnitData data in unitData)
    //    {
    //        if (data.unit == null)
    //            continue;

    //        List<string> unitSynergies = SynergyStringsToList(data.unitStats);

    //        foreach (string synName in unitSynergies)
    //        {
    //            bool duplicateFound = false;
    //            AffectedSynergy reference = null;

    //            foreach (AffectedSynergy syn in affectedSynergies)
    //            {
    //                if (synName == syn.synergy.name)
    //                {
    //                    duplicateFound = true;

    //                    reference = syn;

    //                    break;
    //                }
    //            }

    //            if (duplicateFound)
    //            {
    //                reference.AddNewUnit(data);
    //            }
    //            else
    //            {

    //                Synergy newSynergy = null;

    //                foreach (Synergy syn in SynergyDatabaseScript.Synergies)
    //                {
    //                    if (syn.name == synName)
    //                    {
    //                        newSynergy = syn;
    //                    }
    //                }

    //                if (newSynergy != null)
    //                    affectedSynergies.Add(new AffectedSynergy(newSynergy, data));
    //            }

    //        }
    //    }

    //    foreach (AffectedSynergy affSyn in affectedSynergies)
    //    {
    //        if (affSyn.buffCounter > 0)
    //        {
    //            if (affSyn.synergy.enemyDebuff)
    //            {
    //                affSyn.applyBuffs(hostileArmy);
    //            }
    //            else
    //            {
    //                affSyn.applyBuffs(affSyn.affectedUnits);
    //            }
    //        }
    //        else
    //        {
    //            continue;
    //        }
    //    }
    //}


    protected virtual List<int> SynergyIterationsAffected(List<string> synergyList)
    {
        List<int> iterationsAffected = new List<int>();

        foreach (string synergy in synergyList)
        {
            for (int i = 0; i < synergies.Count; i++)
            {
                if (synergy == synergies[i].synergy.name)
                {
                    iterationsAffected.Add(i);

                    continue;
                }
            }
        }

        return iterationsAffected;
    }
    //protected virtual List<string> SynergyStringsToList(UnitStats unit)
    //{
    //    List<string> synergiesList = new List<string>();

    //    foreach (UnitStats.Trait trait in unit.traits)
    //    {
    //        synergiesList.Add(trait.ToString());
    //    }

    //    foreach (UnitStats.Class _class in unit.classes)
    //    {
    //        synergiesList.Add(_class.ToString());
    //    }

    //    return synergiesList;
    //}
}

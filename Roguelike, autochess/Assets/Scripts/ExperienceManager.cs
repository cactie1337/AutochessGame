using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : Singleton<ExperienceManager>
{
    [Header("Levels")]
    [SerializeField]
    private int currentLevel;
    [SerializeField]
    private int maxLevel;

    [Header("Experience")]
    [SerializeField]
    private int currentExperience;
    [SerializeField]
    private int maxExperience;

    //references
    private ArmyManager armyManagerScript;
    private UIManager userInterface;
    public int CurrentLevel { get => currentLevel; protected set => currentLevel = value; }
    protected int MaxLevel { get => maxLevel; set => maxLevel = value; }
    public int CurrentExperience { get => currentExperience; protected set => currentExperience = value; }
    public int MaxExperience { get => maxExperience; protected set => maxExperience = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }
    protected UIManager UserInterface { get => userInterface; set => userInterface = value; }

    protected virtual void Awake()
    {
        ArmyManagerScript = ArmyManager.Instance;
        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager singleton instance found in the scene. Please add an ArmyManager script to the GameManager gamobject before entering playmode.");
        }
        UserInterface = UIManager.Instance;
        if (!UserInterface)
        {
            Debug.LogError("No UserInterfaceManager singleton instance found in the scene. Please add an UserInterfaceManager script to the GameManager gamobject before entering playmode.");
        }

        if (MaxLevel == 0)
            MaxLevel = 10;

        CurrentLevel = 0;

        CurrentExperience = 0;

        MaxExperience = 1;
    }
    public virtual void GainExperience(int exp)
    {
        if (CurrentLevel == MaxLevel)
            return;
        CurrentExperience += exp;
        if(CurrentExperience >= MaxExperience)
        {
            LevelUp();
        }
        else
        {
            UserInterface.UpdateCurrentExpText(CurrentExperience, MaxExperience);
        }
    }
    protected virtual void LevelUp()
    {
        CurrentLevel += 1;
        CurrentExperience -= MaxExperience;
        IncreaseMaxExp();
        ArmyManagerScript.IncreaseMaxArmySize(1);
        UserInterface.UpdateCurrentLevelText(CurrentLevel);
    }
    protected virtual void IncreaseMaxExp()
    {
        if(CurrentLevel == 3)
        {
            MaxExperience = 0;
        }
        else if (CurrentLevel > 3)
        {
            MaxExperience = 0;
        }

        UserInterface.UpdateCurrentExpText(CurrentExperience, MaxExperience);
    }



}

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
        UserInterface = UIManager.Instance;
        
        if (MaxLevel == 0)
            MaxLevel = 10;

        CurrentLevel = 0;

        CurrentExperience = 0;

        MaxExperience = 1;
    }
}

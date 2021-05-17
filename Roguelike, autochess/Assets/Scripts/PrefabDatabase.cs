using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDatabase : Singleton<PrefabDatabase>
{
    [Header("Unit Prefabs")]
    public GameObject oneStarHealthBar;
    public GameObject twoStarHealthBar;
    public GameObject threeStarHealthBar;
    protected virtual void Awake()
    {
        if (!oneStarHealthBar)
        {
            Debug.LogError("No oneStarHealthBar prefab set in the prefab database! Please set a reference to the oneStarHealthBar prefab in the PrefabDatabase script located on the Databases gameobject" +
                "before entering playmode!");
        }
        if (!twoStarHealthBar)
        {
            Debug.LogError("No twoStarHealthBar prefab set in the prefab database! Please set a reference to the twoStarHealthBar prefab in the PrefabDatabase script located on the Databases gameobject" +
                "before entering playmode!");
        }
        if (!threeStarHealthBar)
        {
            Debug.LogError("No threeStarHealthBar prefab set in the prefab database! Please set a reference to the threeStarHealthBar prefab in the PrefabDatabase script located on the Databases gameobject" +
                "before entering playmode!");
        }
    }
}

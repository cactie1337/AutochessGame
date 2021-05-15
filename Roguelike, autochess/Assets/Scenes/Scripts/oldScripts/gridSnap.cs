using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridSnap : MonoBehaviour
{
    public GameObject target;
    public GameObject structure;
    Vector3 truePos;
    public float gridSize;
    
    void LateUpdate()
    {
        //truePos.x = Mathf.Round(target.transform.position.x);
        //truePos.z = Mathf.Round(target.transform.position.z);
        truePos.y = 1.45F;
        truePos.z = Mathf.Floor(target.transform.position.z / gridSize) * gridSize;
        truePos.x = Mathf.Floor(target.transform.position.x / gridSize) * gridSize;

        structure.transform.position = truePos;
    }
}

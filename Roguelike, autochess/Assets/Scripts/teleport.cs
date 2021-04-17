using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public GameObject Player;
    public Transform[] Target;
    private void OnTriggerEnter(Collider collision)
    {
        var Random = new System.Random();
        int rnd = Random.Next(0, 7);
        print(rnd);
        Player.transform.position = Target[rnd].position;
    }
}

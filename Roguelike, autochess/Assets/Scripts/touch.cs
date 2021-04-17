using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touch : MonoBehaviour
{
    public GameObject daiktas;
    public Text Count;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(daiktas);
        print("Player touched capsule.");
        setCountText(++uiinfo.count);
    }
    private void setCountText(int count)
    {
        Count.text = "Capsules: " + count.ToString();
    }
}

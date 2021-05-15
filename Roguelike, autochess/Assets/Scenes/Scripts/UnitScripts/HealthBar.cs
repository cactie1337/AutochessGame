using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual void Enalble()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}

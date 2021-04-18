using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mv : MonoBehaviour
{
    void OnMouseDrag()
    {
        float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
        //transform.position = new Vector3(Mathf.Floor(pos_move.x / 2) * 2, transform.position.y, Mathf.Floor(pos_move.z / 2) * 2);
        transform.position = new Vector3(Mathf.Round(pos_move.x / 2) * 2, transform.position.y, Mathf.Round(pos_move.z / 2) * 2);
        //Mathf.Floor(target.transform.position.z / gridSize) * gridSize
    }
}

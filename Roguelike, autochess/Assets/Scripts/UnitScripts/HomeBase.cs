using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour
{
    [SerializeField]
    private BoardTile tileScript;
    private Movement movementScript;

    protected BoardTile TileScript { get => tileScript; set => tileScript = value; }
    protected Movement MovementScript { get => movementScript; set => movementScript = value; }

    protected virtual void Awake()
    {
        MovementScript = GetComponent<Movement>();
    }

    public virtual void SetHomeBase(BoardTile tileScript)
    {
        TileScript = tileScript;
    }

    //This will only be called at the end of combat
    public virtual void SendToHomeBase()
    {
        TileScript.ChangeUnitOutOfCombat(gameObject);
    }
}

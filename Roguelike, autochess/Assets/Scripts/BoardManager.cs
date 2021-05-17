using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
   
    //Board
    [SerializeField]
    private Transform boardTransform;
    private Vector3 boardStartPos;

    //board tile prefab
    [SerializeField]
    private GameObject playerBoardTilePrefab;
    [SerializeField]
    private GameObject enemyBoardTilePrefab;

    //active board tile scripts
    [SerializeField]
    private List<BoardTile> boardTiles = new List<BoardTile>();

    private ArmyManager armyManagerScript;


    protected Transform BoardTransform { get => boardTransform; set => boardTransform = value; }
    protected Vector3 BoardStartPosition { get => boardStartPos; set => boardStartPos = value; }
    protected GameObject PlayerBoardTilePrefab { get => playerBoardTilePrefab; set => playerBoardTilePrefab = value; }
    protected GameObject EnemyBoardTilePrefab { get => enemyBoardTilePrefab; set => enemyBoardTilePrefab = value; }
    public List<BoardTile> BoardTiles { get => boardTiles; protected set => boardTiles = value; }
    protected ArmyManager ArmyManagerScript { get => armyManagerScript; set => armyManagerScript = value; }

    protected virtual void Awake()
    {
        if (!BoardTransform)
        {
            Debug.LogError("Please drag your chess board transform into the ChessBoardManager script located on the Game Manager gameobject.");
            return;
        }
        if (!PlayerBoardTilePrefab)
        {
            Debug.LogError("No player chess board tile prefab set in the ChessBoardManager script located on the Game Manager gameobject.");
            return;
        }
        if (!EnemyBoardTilePrefab)
        {
            Debug.LogError("No enemy chess board tile prefab set in the ChessBoardManager script located on the Game Manager gameobject.");
            return;
        }
        ArmyManagerScript = ArmyManager.Instance;
        if (!ArmyManagerScript)
        {
            Debug.LogError("No ArmyManager script found, please add one to the game manager gameobject.");
        }

        CreateBoardTiles();
    }

    protected virtual void CreateBoardTiles()
    {
        int counter = 0;

        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                if(z < 4)
                {
                    GameObject tile = Instantiate(playerBoardTilePrefab, BoardTransform);
                    tile.name = "Player Tile " + counter.ToString() + " Position: (" + x.ToString() + ", " + z.ToString() + ")";
                    tile.transform.localPosition = new Vector3(x * 5, 0, z * 5);
                    BoardTile tileScript = tile.GetComponent<BoardTile>();
                    tileScript.Setup(counter, new Vector2(x, z), BoardTile.TileCategory.Player);
                    BoardTiles.Add(tileScript);
                }
                else
                {
                    GameObject tile = Instantiate(enemyBoardTilePrefab, BoardTransform);
                    tile.name = "Enemy Tile " + counter.ToString() + " Position: (" + x.ToString() + ", " + z.ToString() + ")";
                    tile.transform.localPosition = new Vector3(x * 5, 0, z * 5);
                    BoardTile tileScript = tile.GetComponent<BoardTile>();
                    tileScript.Setup(counter, new Vector2(x, z), BoardTile.TileCategory.Enemy);
                    BoardTiles.Add(tileScript);
                }
                counter++;            
            }
        }
    }
}

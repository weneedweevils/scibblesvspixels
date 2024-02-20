using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;
    public TileBase wallTile;
    public Grids.MapGrid mygrid;
    public bool initialized = false;

    private GameObject player;
    private GameObject crab;
    private EnemyMovement enemymovement;
    private Dictionary<Vector2Int, OverlayTile> map;
    private int frames = 0;





    private void Awake()
    {
        if(_instance != null && _instance == this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        player = GameObject.Find("Player");
        crab = GameObject.Find("DoodleCrab");
       
    }

    private void Start()
    {
        map = new Dictionary<Vector2Int, OverlayTile>();
        var somedTileMap = gameObject.GetComponentInChildren<Tilemap>();
        var tileMap = GameObject.Find("Ground - 0").GetComponent<Tilemap>();

        tileMap.CompressBounds();

      

        BoundsInt bounds = tileMap.cellBounds;

        int rows = bounds.max.x - bounds.min.x;
        int cols = bounds.max.y - bounds.min.y;


        mygrid = new Grids.MapGrid(rows,cols);
        

        // get all tiles in current tile map
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
                


                int corrected_x = -(bounds.min.x) + x;
                int corrected_y = -(bounds.min.y) + y;

                Vector2Int tilekey = new Vector2Int(corrected_x, corrected_y);


                if (tileMap.HasTile(tileLocation) && somedTileMap.GetTile(tileLocation) != wallTile) 
                {


                    var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);


                    overlayTilePrefab.gridpos = new Vector2Int(corrected_x, corrected_y);
                    overlayTilePrefab.mappos = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);

                    float actualx = overlayTilePrefab.mappos.x;
                    float actualy = overlayTilePrefab.mappos.y;

                    overlayTile.gridcell = new Grids.GridCell(corrected_x, corrected_y,actualx, actualy);
                    
                   
                    //overlayTile.name = overlayTilePrefab.gridpos.ToString();

                    overlayTile.name = $"Tile ({overlayTile.gridcell.getX()}, {overlayTile.gridcell.getY()}) {overlayTile.GetType()}";
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);
                    //overlayTile.transform.SetParent(overlayContainer.transform);

                    //Debug.Log($"my tile key is or uncorrected gridpos is {tilekey}");
                    //Debug.Log($"my corrected gridspos is ({corrected_x},{corrected_y})");
                    //Debug.Log($"my mappos is {overlayTile.mappos}");
                    //Debug.Log($"my actial x and y is ({actualx},{actualy})");

                    //Debug.Log($"Default: ({x},{y})");
                    //Debug.Log($"Corrected: ({corrected_x},{corrected_y})");
                    mygrid.addcell(overlayTile.gridcell); // add tile to the grid
                    //Debug.Log(tilekey);
                    //Debug.Log(overlayTile);
                    map.Add(tilekey, overlayTile);

                    //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                }
                
            }
        } // end of grid generation

        // add a script to the crab and then get the script
        //crab.AddComponent<EnemyMovement>();
        enemymovement = crab.GetComponent<EnemyMovement>();

        Vector2 playerPos = player.transform.position;
        Vector2 crabPos = crab.transform.position;

        Vector3Int playerCellPos = tileMap.WorldToCell(playerPos);
        Vector3Int crabCellPos = tileMap.WorldToCell(crabPos);

        int playerGridX = playerCellPos.x - bounds.min.x;
        int playerGridY = playerCellPos.y - bounds.min.y;

        int crabGridX = crabCellPos.x - bounds.min.x;
        int crabGridY = crabCellPos.y - bounds.min.y;

        Debug.Log("Player grid coordinates: (" + playerGridX + ", " + playerGridY + ")");
        Debug.Log("Crab grid coordinates: (" + crabGridX + ", " + crabGridY + ")");


       



        }










    private void Update()
    {
        frames++;
        if (frames % 600 == 0)
        {

            var tileMap = GameObject.Find("Ground - 0").GetComponent<Tilemap>();
            BoundsInt bounds = tileMap.cellBounds;
            Vector2 playerPos = player.transform.position;
            Vector2 crabPos = crab.transform.position;

            Vector3Int playerCellPos = tileMap.WorldToCell(playerPos);
            Vector3Int crabCellPos = tileMap.WorldToCell(crabPos);

            int playerGridX = playerCellPos.x - bounds.min.x;
            int playerGridY = playerCellPos.y - bounds.min.y;

            int crabGridX = crabCellPos.x - bounds.min.x;
            int crabGridY = crabCellPos.y - bounds.min.y;
            List<Grids.GridCell> path;
            var step = 1 * Time.deltaTime;

            OverlayTile nextTile;









            //Debug.Log("Player grid coordinates: (" + playerGridX + ", " + playerGridY + ")");
            //Debug.Log("Crab grid coordinates: (" + crabGridX + ", " + crabGridY + ")");

            //enemyScript.hello();

            if (!enemymovement.created)
            {
                enemymovement.DelayedStart(mygrid);

            }
            //else
            //{



            path = enemymovement.CalculatePathToTarget(crabGridX, crabGridY, playerGridX, playerGridY);
            //if (path != null && path.Count > 0)
            //{
            //    int stringx = path[path.Count].getX();
            //    int stringy = path[path.Count].getY();












            //    Vector2Int tileposition = new Vector2Int(stringx, stringy);
            //    nextTile = map[tileposition];






            //Debug.Log($"Moving to {nextTile.mappos}");

            //crab.transform.position = Vector2.MoveTowards(crab.transform.position, nextTile.mappos, step);



            //for (int i = path.Count - 1; i >= 0; i--)
            //{
            //    Debug.Log($"Next node is {path[i]}");
            //    int stringx = path[i].getX();
            //    int stringy = path[i].getY();
            //    Vector2Int tileposition = new Vector2Int(stringx, stringy);
            //    nextTile = map[tileposition];
            //    Debug.Log($"Moving to {nextTile.mappos}");
            //    //crab.transform.position = Vector2.MoveTowards(crab.transform.position, nextTile.mappos, step);
            //    StartCoroutine(waiter(nextTile.mappos, step));
            //}
            //crab.transform.position = nextTile.mappos;
        }



    }



    //}


    //if(path!=null && path.Count > 0)
    //{
    //    for (int i = path.Count - 1; i >= 0; i--)
    //    {

    //        Vector2Int nextMove = new Vector2Int(path[i].getX(), path[i].getY());

    //        Debug.Log($"my actual move is{nextMove}");
    //        //Debug.Log($"my corrected move is{correctedMove}");



    //        crab.transform.position = Vector2.MoveTowards(crab.transform.position, nextMove, Time.deltaTime * 1);
    //    }

    //}










    IEnumerator waiter(Vector3 item, float step)
    {
        crab.transform.position = Vector3.Lerp(crab.transform.position, item, step);
        yield return new WaitForSeconds(4);
    }


    public Grids.MapGrid GetMap(){
        return mygrid;
    }

  
}


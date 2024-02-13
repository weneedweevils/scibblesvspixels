using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;
    public TileBase wallTile;
    public MapGrid mygrid;


   

    public class MapGrid
    {
        Grids.GridCell[,] newestgrid;
        private int _rows;
        private int _cols;


        public MapGrid(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            newestgrid = new Grids.GridCell[_rows, _cols];
        }

        // this function will add a new grid cell to the gridmap

        public void addcell(Grids.GridCell mycell)
        {
            //Debug.Log($"{mycell.getX()}, {mycell.getY()}\n\n");
            newestgrid[mycell.getX(), mycell.getY()] = mycell;
        }

        public void SuperString()
        {
            string superstring = "";
            for (int y = 0; y < _cols; y++)
            {
                for (int x = 0; x < _rows; x++)
                {
                    if (newestgrid[x,y] == null)
                    {
                        superstring += "0";
                    }
                    else
                    {
                        superstring += "*";
                    }
                }
                superstring += "\n";
            }
            Debug.Log(superstring);
        }   
    }




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

        
    }

    private void Start()
    {
        var somedTileMap = gameObject.GetComponentInChildren<Tilemap>();
        var tileMap = GameObject.Find("Ground - 0").GetComponent<Tilemap>();

        tileMap.CompressBounds();

      

        BoundsInt bounds = tileMap.cellBounds;

        int rows = bounds.max.x - bounds.min.x;
        int cols = bounds.max.y - bounds.min.y;


        mygrid = new MapGrid(rows,cols);
        

        // get all tiles in current tile map
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                var tileLocation = new Vector3Int(x, y, 0);


                int corrected_x = -(bounds.min.x) + x;
                int corrected_y = -(bounds.min.y) + y;

                //if (tileMap.GetTile(tileLocation) == wallTile)
                //{
                //   Debug.Log(x.ToString() + "," + y.ToString());
                //}

                //Debug.Log(x.ToString() + "," +y.ToString());

                //if (tileMap.GetTile(tileLocation) == wallTile)
                //{
                //    Debug.Log("GO TO SLEEP");
                //}
                
                    
                if (tileMap.HasTile(tileLocation) && somedTileMap.GetTile(tileLocation) != wallTile) 
                {

                   

                    var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                    overlayTile.prefabAsset = new Grids.GridCell(corrected_x, corrected_y);
                    overlayTile.name = $"Tile ({overlayTile.prefabAsset.getX()}, {overlayTile.prefabAsset.getY()}) {overlayTile.GetType()}";
                    overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, 0);
                    overlayTile.transform.SetParent(overlayContainer.transform);


                    

                   
                    

                    //Debug.Log($"Default: ({x},{y})");
                    //Debug.Log($"Corrected: ({corrected_x},{corrected_y})");
                    mygrid.addcell(overlayTile.prefabAsset);

                    //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                }
                
            }
        }
  



    }

  
}


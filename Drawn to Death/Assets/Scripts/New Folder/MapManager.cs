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
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        tileMap.CompressBounds();

      



        BoundsInt bounds = tileMap.cellBounds;
        Debug.Log(bounds.min.x);
        Debug.Log(bounds.max.x);
        Debug.Log(bounds.min.y);
        Debug.Log(bounds.max.y);

        // get all tiles in current tile map
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                var tileLocation = new Vector3Int(x, y, 0);
          

              

                //if (tileMap.GetTile(tileLocation) == wallTile)
                //{
                //   Debug.Log(x.ToString() + "," + y.ToString());
                //}

                //Debug.Log(x.ToString() + "," +y.ToString());

                if (tileMap.GetTile(tileLocation) == wallTile)
                {
                    Debug.Log("GO TO SLEEP");
                }
                
                    
                if (tileMap.HasTile(tileLocation) ) 
                {

                   

                    var overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
                   
                    var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

                    overlayTile.transform.position = new Vector3(cellWorldPosition.x+10, cellWorldPosition.y+10, 0);

                    //overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;

                }
                
            }
        }
        

    }
}

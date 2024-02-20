using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
  


    private GameObject mymap; 
    private Grids.Pathfinding astar;
    private List<Grids.GridCell> glichpath;
    public bool created = false;
    Rigidbody2D rbody;
    Tilemap tileMap;
    private GameObject player;
    Vector3 playerPos;
    BoundsInt bounds;
    private int frames = 0;



    // get the map from the mapmanager
    private void Start(){
       rbody = GetComponent<Rigidbody2D>();
       tileMap = GameObject.Find("Ground - 0").GetComponent<Tilemap>();
       player = GameObject.Find("Player");
       bounds= tileMap.cellBounds;


    }


    public void DelayedStart(Grids.MapGrid astarmap)
    {
        mymap = GameObject.Find("Grid");
        astar = new Grids.Pathfinding(astarmap);

        //astar.map = mymap.GetComponent<MapManager>().GetMap();
        //astar.map = astarmap;

        if (astar.getGrid() == null)
        {
            Debug.Log("we have not set our map");
        }
        else
        {
            
            Debug.Log("we have set our map");
        }
        created = true;
    }

    private void Update()
    {
     
      
        //    Vector3Int playerCellPos = tileMap.WorldToCell(playerPos);
        //    Vector3Int crabCellPos = tileMap.WorldToCell(transform.position);


        //    int playerGridX = playerCellPos.x - bounds.min.x;
        //    int playerGridY = playerCellPos.y - bounds.min.y;
        //    int crabGridX = crabCellPos.x - bounds.min.x;
        //    int crabGridY = crabCellPos.y - bounds.min.y;

        //    glichpath = astar.findPath(crabGridX, crabGridY, playerGridX, playerGridY);
            //glichpath = CalculatePathToTarget(crabGridX, crabGridY, playerGridX, playerGridY);

            if (glichpath != null)
            {
                MoveCrab();
            }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Grids.GridCell prev = glichpath[0];

        foreach (Grids.GridCell cell in glichpath)
        {
            Vector3 prvworldposition = new Vector3(prev.getActualx(), prev.getActualy(), 0);
            Vector3 worldposition = new Vector3(cell.getActualx(), cell.getActualy(), 0);

            //Gizmos.DrawLine(transform.position, target.position);
            Gizmos.DrawLine(prvworldposition, worldposition);
            prev = cell;
        }
    }



    public List <Grids.GridCell> CalculatePathToTarget(int startx, int starty, int targetx, int targety)
    {
      
        //Debug.Log(transform.position
        if (astar.map == null)
        {
            Debug.Log("There is no map");
            return null;
        }
        else
        {
            //astar.map.SuperString
            Debug.Log("we are fine");
            glichpath = astar.findPath(startx, starty, targetx, targety);
            //Debug.Log(glichpath.Count);
            return glichpath;
        }


     


        // move to each position



        
    }


    public void MoveCrab()
    {

        //Rigidbody rbody = GetComponent<Rigidbody>();
        Vector2 currentPos = rbody.position;
        Vector2 targetPos = glichpath[glichpath.Count-1].ReturnWorldPos();  //Last point in the path array
        Vector2 velocity = (targetPos - currentPos).normalized * 1 * Time.fixedDeltaTime;

        //Predict new position
        Vector2 newPos;
        
        if (Vector2.Distance(currentPos, targetPos) < velocity.magnitude)
        {
            newPos = targetPos;
            //Remove last point in the path array
            glichpath.RemoveAt(glichpath.Count - 1);

        }
        else
        {
            newPos = currentPos + velocity;
        }

        //Move to new position
        rbody.MovePosition(newPos);

    }


    public void hello()
    {
        Debug.Log("hello world");
    }

    //public RaycastHit2D? GetFocusedTile()
    //{
    //    Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    Vector2 mousepos2d = new Vector2(mousepos.x, mousepos.y);

    //    RaycastHit2D[] hits = Physics2D.RaycastAll(mousepos2d, Vector2.zero);

    //    if (hits.Length > 0)
    //    {
    //        return hits.OrderByDescending(i => i.collider.transform.position.z).First();
    //    }
    //    return null;
    //}
}

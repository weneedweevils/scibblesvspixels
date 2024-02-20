using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{

    public Grids.GridCell gridcell;
    public Vector2 gridpos; // position used for the grid
    public Vector3 mappos; // position used for actually moving in space
    //public EnemyMovement enemyMovement;
    public GameObject crab;
    private EnemyMovement enemymovement;


    //void Update()
    //{
   //     if (Input.GetMouseButtonDown(0))
     //   {
    //        HideTile();
   //     }
       
  //  }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

}






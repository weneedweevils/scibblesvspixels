using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // checks to see if we clicked on tile with mouse
            var focusedTileHit = GetFocusedTile();
            if (focusedTileHit.HasValue)
            {
                GameObject overlayTile = focusedTileHit.Value.collider.gameObject;
                //Debug.Log(overlayTile);



                transform.position = overlayTile.transform.position;
                Debug.Log(overlayTile);
            }
        }
    }


    public RaycastHit2D? GetFocusedTile()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousepos2d = new Vector2(mousepos.x, mousepos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousepos2d, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }
        return null;
    }
}

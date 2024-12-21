using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    public Grid grid;
    public Vector3 offset;

    private void OnDrawGizmosSelected()
    {
        transform.position = Snap(transform.position - offset, grid) + offset;
    }

    public static Vector3 Snap(Vector3 pos, Grid grid)
    {
        return grid.CellToWorld(grid.WorldToCell(pos));
    }
}

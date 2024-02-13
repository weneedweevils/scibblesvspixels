using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Grids
{
    public class GridCell
    {
        private int _x;
        private int _y;



        public GridCell(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int getX()
        {
            return _x;
        }
        public int getY()
        {
            return _y;
        }
    }
}

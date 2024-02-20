using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Diagnostics;
using UnityEngine;

public static class Grids
{
    public class GridCell
    {
        private int _x;
        private int _y;
        private float _actualx;
        private float _actualy;


        public int gCost;
        public int hCost;
        public int fCost;

        public GridCell cameFromNode;



        public GridCell(int x, int y,float actual_x, float actual_y)
        {
            _x = x;
            _y = y;
            _actualx = actual_x;
            _actualy = actual_y;
        }

        public int getX()
        {
            return _x;
        }
        public int getY()
        {
            return _y;
        }

        public float getActualx()
        {
            return _actualx;
        }

        public float getActualy()
        {
            return _actualy;
        }

        public void CalculateFcost(){
            fCost = gCost + hCost;
        }

        public Vector2 ReturnWorldPos()
        {
            return new Vector2(_actualx, _actualy);
            
        }

        public override string ToString() {

            string formatString = $"x coordinate: {_x}, y coordinate: {_y}\n gcost: {gCost} \n hcost: {hCost} \n fcost: {fCost}";
            return formatString;
        }

       
        

        

    }



 

    public class MapGrid
    {
        public GridCell[,] array2d;
        private int _rows;
        private int _cols;


        public MapGrid(int rows, int cols)
        {
            _rows = rows;
            _cols = cols;
            array2d = new GridCell[_rows, _cols];
        }

        // this function will add a new grid cell to the gridmap

        public void addcell(GridCell mycell)
        {
            //Debug.Log($"{mycell.getX()}, {mycell.getY()}\n\n");
            array2d[mycell.getX(), mycell.getY()] = mycell;
        }

        public void SuperString()
        {
            string superstring = "";
            for (int y = 0; y < _cols; y++)
            {
                for (int x = 0; x < _rows; x++)
                {
                    if (array2d[x,y] == null)
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

        public GridCell GetGridCell(int x, int y){
            return array2d[x,y];
        }

        public int getRows(){
            return _rows;
        }

        public int getColumns(){
            return _cols;
        }
       
       public bool isValid(int x, int y){
            if((x<_rows && x >= 0) 
                && (y < _cols && y >= 0)
                && array2d[x, y] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
          


       }
    }


    public class Pathfinding{
        public MapGrid map;
        public List<GridCell> openList;
        private List<GridCell> closedList;
        

        public Pathfinding(MapGrid newmap)
        {
            map = newmap;
   

        }


        public MapGrid getGrid()
        {
            return map;
        }

        public List<GridCell> findPath(int startx, int starty, int endx, int endy){
            //map.SuperString();

            Debug.Log($"started: ({startx}, {starty}) ({endx}, {endy})");
            GridCell startNode = map.GetGridCell(startx,starty);
            GridCell endNode = map.GetGridCell(endx,endy);
            GridCell closestNode = startNode;

        openList = new List<GridCell> {startNode};
            closedList = new List<GridCell>();

            



            // initialize path finding so all values are set to infinity
            for (int x=0; x<map.getRows(); x++){
                for(int y = 0; y<map.getColumns(); y++){
                    if(map.isValid(x,y)){
                        GridCell node = map.GetGridCell(x,y);
                        node.gCost = int.MaxValue;
                        node.CalculateFcost();
                        node.cameFromNode = null;

                    }
                }

            }

            startNode.gCost = 0;
            startNode.hCost = CalculateHeuristic(startNode, endNode);
            startNode.CalculateFcost();

            Debug.Log("startnode after calculations");
            Debug.Log(startNode);

            while (openList.Count>0){
                GridCell currentNode = getLowestCost(openList);
                if (currentNode.hCost<2){
                    Debug.Log("Found a Path!");
                    return CalculatePath(currentNode);
                }
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach(GridCell neighbourNode in GetNeighboursList(currentNode)){
                    if(closedList.Contains(neighbourNode)) continue;

                    int tentativeGcost = currentNode.gCost + CalculateDistance(currentNode,neighbourNode);
                    if (tentativeGcost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGcost;
                        neighbourNode.hCost = CalculateHeuristic(neighbourNode, endNode);
                        neighbourNode.CalculateFcost();

                        if (neighbourNode.hCost < closestNode.hCost)
                        {
                            closestNode = neighbourNode;
                        }

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }

                    }
                }


            }

            //Debug.Log("This is current node");
            //Debug.Log(currentNode);
            Debug.Log("This is the CLOSEST node");
            Debug.Log(closestNode);
            Debug.Log("No Path found :(");
            return null;
        }

        private List<GridCell> GetNeighboursList(GridCell currentNode){
            List<GridCell> neigbourList = new List<GridCell>();

            for(int i = -1; i<2;i++){
                for(int j = -1; j<2; j++){
                    if(i!=0 & j!=0){
                        if(map.isValid(currentNode.getX()+i,currentNode.getY()+j)){
                            neigbourList.Add(map.GetGridCell(currentNode.getX() + i, currentNode.getY() + j));
                                                  
                        }
                    }
                }
            }

            return neigbourList;
        }

        
        private List<GridCell> CalculatePath(GridCell endNode){
            List<GridCell> path = new List<GridCell>();
            path.Add(endNode);
            GridCell currentNode = endNode;
            while(currentNode.cameFromNode != null) {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
             }
            return path;
        }


        private int CalculateDistance(GridCell a, GridCell b){
            int xDistance = Mathf.Abs(a.getX()-b.getX());
            int yDistance = Mathf.Abs(a.getY()-b.getY());
            int remaining = Mathf.Abs(xDistance - yDistance);
            return (2 * Mathf.Min(xDistance, yDistance) + 1 * remaining);
        }
        private int CalculateHeuristic(GridCell a, GridCell b){
            int xDistance = Mathf.Abs(a.getX()-b.getX());
            int yDistance = Mathf.Abs(a.getY()-b.getY());
            int remaining = Mathf.Abs(xDistance - yDistance);
            return 2 * Mathf.Min(xDistance, yDistance) + 1 * remaining;
        }


        private GridCell getLowestCost(List<GridCell> nodeList){
           
           GridCell lowest = nodeList[0];
           for(int i =0;i<nodeList.Count;i++){
               if(nodeList[i].fCost<lowest.fCost){
                   lowest = nodeList[i];

               }
            }

            return lowest;
        }
    }
}

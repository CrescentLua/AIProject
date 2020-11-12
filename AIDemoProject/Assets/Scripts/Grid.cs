using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask blockadeMask; 
    public float radiusNode; //Radius of the node
    public Vector2 worldSizeGrid; //The grid's world size 

    Node[,] grid; //Create a grid of nodes consisting of a 2D array 

    Vector2 gridSize;
    float diameterNode; //Diameter of the node 

    Vector3 bottomLeftWorld; //The bottom left of the world   

    void Start()
    {
        worldSizeGrid.x = 30.0f; //Creating a 30x30 sized grid 
        worldSizeGrid.y = 30.0f;
        radiusNode = 0.5f; //With a node radius of 0.5; Can make this smaller or bigger, but the collision on the layerMask will vary!  

        diameterNode = radiusNode * 2; //Retrieve the diameter of the node 
        SetGridSize();
        BuildGrid();
    }

    void SetGridSize()
    {
        gridSize.x = Mathf.RoundToInt(worldSizeGrid.x / diameterNode); //This will determine how many nodes can fit within the X, and Y coordinates
        gridSize.y = Mathf.RoundToInt(worldSizeGrid.y / diameterNode); //Using RoundToInt method to make sure the variable is an int 
    }

    void Update()
    {   

    } 

    void BuildGrid()
    {
        grid = new Node[(int)gridSize.x, (int)gridSize.y]; 
        bottomLeftWorld = (transform.position - Vector3.right) * (worldSizeGrid.x / 2.0f) - (Vector3.forward * worldSizeGrid.y) / 2.0f;

        for (int gridX = 0; gridX < gridSize.x; gridX++)
        {
            for (int gridY = 0; gridY < gridSize.y; gridY++)
            {
                Vector3 pointInWorld = bottomLeftWorld + Vector3.right * (gridX * diameterNode + radiusNode) + Vector3.forward * (gridY * diameterNode + radiusNode);
                bool canWalk; 

                if (Physics.CheckSphere(pointInWorld, radiusNode, blockadeMask))
                {
                    canWalk = false; 
                }

                else
                {
                    canWalk = true; 
                }

                grid[gridX, gridY] = new Node(canWalk, pointInWorld, new Vector2(gridX, gridY)); 
            }
        }
    }

    public List<Node> GetNeighbouringNodes(Node node_)
    {
        List<Node> neighbouringNodes = new List<Node>(); 

        //For loop that searches in a 3x3 block surrounding the node 
        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                Vector2 check;

                check.x = (int)node_.gridPosition.x + x;
                check.y = (int)node_.gridPosition.y + y; 

                //Check if these variables are within the grid 
                if (check.x >= 0 && check.x < gridSize.x && check.y >= 0 && check.y < gridSize.y)
                {
                    neighbouringNodes.Add(grid[(int)check.x, (int)check.y]);
                }
            }
        }

        return neighbouringNodes;
    }

    public Node GetNodeFromWorldPosition(Vector3 AIPos_)
    {
        Vector2 percentage; 
        percentage.x = (AIPos_.x + worldSizeGrid.x / 2.0f) / worldSizeGrid.x;
        percentage.y = (AIPos_.z + worldSizeGrid.y / 2.0f) / worldSizeGrid.y;

        percentage.x = Mathf.Clamp01(percentage.x);
        percentage.y = Mathf.Clamp01(percentage.y);

        Vector2 gridInfo;
        gridInfo.x = Mathf.RoundToInt((gridSize.x - 1) * percentage.x);
        gridInfo.y = Mathf.RoundToInt((gridSize.y - 1) * percentage.y);

        return grid[(int)gridInfo.x, (int)gridInfo.y];
    }

    public List<Node> path;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSizeGrid.x , 1.0f, worldSizeGrid.y));

        if (grid != null)
        {
            foreach (Node node in grid)
            {
                if (node.canWalk)
                {
                    Gizmos.color = new Color(0, 1, 0, 1); //If the node is walkable, set the colour to green 
                }
                
                else
                {
                    Gizmos.color = new Color(1, 0, 0, 1); //Otherwise if the node cannot be walked on, set the colour to red.
                }

                if (path != null)
                {
                    if(path.Contains(node))
                    {
                        Gizmos.color = new Color(0, 0, 1, 1);
                    }
                } 

                float gridSeparation = 0.1f; //This just helps separate each node on the grid when they are drawn so they don't look glued together
                Gizmos.DrawCube(node.worldPosition, new Vector3(1.0f, 1.0f, 1.0f) * (diameterNode - gridSeparation));
            } 
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform AI;

    public LayerMask blockadeMask; 
    public float radiusNode; //Radius of the node
    public Vector2 worldSizeGrid; //The grid's world size 

    Node[,] grid;

    Vector2 gridSize;
    float diameterNode; //Diameter of the node 

    Vector3 bottomLeftWorld; //The world's bottom left   

    void Start()
    {
        worldSizeGrid.x = 30.0f; //Creating a 30x30 sized grid 
        worldSizeGrid.y = 30.0f;
        radiusNode = 0.5f; //With a node radius of 0.5; Can make this smaller or bigger, but the collision on the layerMask will vary!  

        diameterNode = radiusNode * 2; //Retrieve the diameter of the node 
        SetGridSize(); 
    }

    void SetGridSize()
    {
        gridSize.x = Mathf.RoundToInt(worldSizeGrid.x / diameterNode);
        gridSize.y = Mathf.RoundToInt(worldSizeGrid.y / diameterNode);
    }

    void Update()
    {
        BuildGrid();     
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

                grid[gridX, gridY] = new Node(canWalk, pointInWorld); 
            }
        }
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

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(worldSizeGrid.x , 1.0f, worldSizeGrid.y));

        if (grid != null)
        {
            Node AINode = GetNodeFromWorldPosition(AI.position); 
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

                if (AINode == node)
                {
                    Gizmos.color = new Color(0, 0, 1, 1); //Otherwise if the node cannot be walked on, set the colour to red.
                }


                float gridSeparation = 0.1f; //This just helps separate each node on the grid when they are drawn so they don't look glued together
                Gizmos.DrawCube(node.worldPosition, new Vector3(1.0f, 1.0f, 1.0f) * (diameterNode - gridSeparation));
            } 
        }
    }
}

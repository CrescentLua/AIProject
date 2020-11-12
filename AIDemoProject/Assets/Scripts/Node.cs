using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool canWalk; //A boolean variable to decide if the node is walkable or not  
    public Vector3 worldPosition; //This represents the node's point within the world  
    public Vector2 gridPosition; 

    public int gCost;
    public int hCost;

    public Node parent; 

    public Node(bool canWalk_, Vector3 worldPosition_, Vector2 gridPosition_) //A constructor to assign both variables 
    {
        canWalk = canWalk_;
        worldPosition = worldPosition_;
        gridPosition.x = gridPosition_.x;
        gridPosition.y = gridPosition_.y; 
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}

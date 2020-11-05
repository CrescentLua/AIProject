using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public bool canWalk; //A boolean variable to decide if the node is walkable or not  
    public Vector3 worldPosition; //This represents the node's point within the world  

    public Node(bool canWalk_, Vector3 worldPosition_) //A constructor to assign both variables 
    {
        canWalk = canWalk_;
        worldPosition = worldPosition_;
    }
}

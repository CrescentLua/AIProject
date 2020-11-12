using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    public Transform AI;
    public Transform Target; 
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        PathFind(AI.position, Target.position);
    }

    void PathFind(Vector3 startPosition_, Vector3 targetPosition_)
    {
        Node startingNode = grid.GetNodeFromWorldPosition(startPosition_);  //Convert these world positions into nodes 
        Node targetNode = grid.GetNodeFromWorldPosition(targetPosition_);

        List<Node> openSet = new List<Node>(); //Set of nodes that need to be evaluated
        HashSet<Node> closedSet = new HashSet<Node>(); //Set of nodes that are already evaluted 

        openSet.Add(startingNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0]; 
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                ReDiscoverPath(startingNode, targetNode);
                return; 
            }

            foreach (Node neighbour in grid.GetNeighbouringNodes(currentNode))
            {
                if(neighbour.canWalk == false || closedSet.Contains(neighbour))
                {
                    continue; 
                }

                //Retrieve an integer from the current node to the neighbour node 
                int newCostToNeighbour = currentNode.gCost + GetNodesDistance(currentNode, neighbour);   

                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetNodesDistance(neighbour, targetNode);
                    neighbour.parent = currentNode; //Set the parent of the neighbour to the current node 

                    //If the neighbour node is not in the openSet, then add it into the set
                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

    }

    void ReDiscoverPath(Node startNode_, Node finalNode_)
    {
        List<Node> newPath = new List<Node>();
        Node currentNode = finalNode_; 

        while (currentNode != startNode_)
        {
            newPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        newPath.Reverse();

        grid.path = newPath;
    }

    int GetNodesDistance(Node node1_, Node node2_)
    {
        int distanceAlongX = Mathf.Abs((int)node1_.gridPosition.x - (int)node2_.gridPosition.x);
        int distanceAlongY = Mathf.Abs((int)node1_.gridPosition.y - (int)node2_.gridPosition.y); 

        if (distanceAlongX > distanceAlongY)  
            return (14 * distanceAlongY + 10) * (distanceAlongX - distanceAlongY); 

        return (14 * distanceAlongX + 10) * (distanceAlongY - distanceAlongX);
    }
}

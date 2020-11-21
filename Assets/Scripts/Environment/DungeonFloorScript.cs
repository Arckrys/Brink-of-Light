using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFloorScript : MonoBehaviour
{
    private static DungeonFloorScript _instance;
    private List<FloorNode> nodeList;

    private int basicRoomsNumber = 3;
    private int currentNodeIndex;

    private void Start()
    {
        nodeList = new List<FloorNode>();

        //create a first room
        FloorNode newNode = new FloorNode();
        nodeList.Add(newNode);

        //create the number of rooms desired
        while (basicRoomsNumber > 0)
        {
            createRandomNode();
            basicRoomsNumber--;
        }

        //debug print
        foreach (FloorNode node in nodeList)
            print(node.SetRoom());
        
        //initialize the scene in the first room;
        InitializeMap(nodeList[0].GetRoom());
        currentNodeIndex = 0;
    }

    public FloorNode GetCurrentNode()
    {
        return nodeList[currentNodeIndex];
    }

    public void SetCurrentNode(FloorNode node)
    {
       currentNodeIndex = nodeList.IndexOf(node);
    }

    public static DungeonFloorScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<DungeonFloorScript>();
            }

            return _instance;
        }
    }

    private void createRandomNode()
    {
        bool canBeCreated = false;

        //debug count
        int count = 0;

        while(!canBeCreated && count < 1000)
        {
            count++;

            //take an existing room
            FloorNode existingNode = nodeList[Random.Range(0, nodeList.Count)];

            //choose a random direction
            var randomDirection = (FloorNode.directionEnum)Random.Range(0, 3);

            //if the existing room has no room at the direction chosen we create the room there
            if (existingNode.GetNeighbourNode(randomDirection) == null)
            {
                FloorNode newNode = new FloorNode();

                //add the new room as a neighbour of the existing room
                existingNode.SetNeighbourNode(randomDirection, newNode);
                print(randomDirection);

                //add the existing room as a neighbour of the new room
                newNode.SetNeighbourNode(GetOppositeDirection(randomDirection), existingNode);
                print(GetOppositeDirection(randomDirection));

                nodeList.Add(newNode);
                canBeCreated = true;
            }
        }
    }

    private FloorNode.directionEnum GetOppositeDirection(FloorNode.directionEnum direction)
    {
        //return the opposite direction of a given direction
        int newDirection = ((int)direction + 2) % 4;
        return (FloorNode.directionEnum)newDirection;
    }

    public void InitializeMap(Object mapPrefab)
    {
        Instantiate(mapPrefab);
    }
}

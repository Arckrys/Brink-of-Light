using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFloorScript : MonoBehaviour
{
    private static DungeonFloorScript _instance;
    private List<FloorNode> nodeList;

    private int basicRoomsNumber = 25;
    private int currentNodeIndex;

    private void Start()
    {
        nodeList = new List<FloorNode>();

        //create a first room
        FloorNode newNode = new FloorNode();
        newNode.SetRoomType(FloorNode.roomTypeEnum.regular);
        newNode.SetCoord(0, 0);
        nodeList.Add(newNode);

        //create the number of rooms desired
        while (basicRoomsNumber > 0)
        {
            createRandomNode(FloorNode.roomTypeEnum.regular);
            basicRoomsNumber--;
        }

        AddSpecialRooms();

        foreach (FloorNode node in nodeList)
        {
            node.SetRoom(node.GetRoomType());
            node.CreateRoom();
            node.ActivateRoom(false);
        }

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

    private void createRandomNode(FloorNode.roomTypeEnum roomType)
    {
        bool canBeCreated = false;

        //debug count
        int count = 0;

        while(!canBeCreated && count < 1000)
        {
            count++;

            //if we create a regular room
            if (roomType == FloorNode.roomTypeEnum.regular)
            {

                //take an existing room
                FloorNode existingNode = nodeList[Random.Range(0, nodeList.Count)];

                //choose a random direction
                var randomDirection = (FloorNode.directionEnum)Random.Range(0, 3);

                //if the existing room has no room at the direction chosen we create the room there and is not a special room
                if (existingNode.GetRoomType() == FloorNode.roomTypeEnum.regular && existingNode.GetNeighbourNode(randomDirection) == null)
                {
                    FloorNode newNode = new FloorNode();

                    newNode.SetRoomType(roomType);

                    //connect the new room with the existing room
                    CreateLinkBetweenNodes(existingNode, newNode, randomDirection);

                    //set the new room coord based on the existing room we used to place it
                    (int x, int y) = existingNode.GetCoord();

                    if (randomDirection == FloorNode.directionEnum.east)
                        x += 1;
                    if (randomDirection == FloorNode.directionEnum.west)
                        x -= 1;
                    if (randomDirection == FloorNode.directionEnum.north)
                        y += 1;
                    if (randomDirection == FloorNode.directionEnum.south)
                        y -= 1;

                    newNode.SetCoord(x, y);

                    //check if there are rooms existing around the new room and connect them.
                    //This allow the level to have loops and not look like a tree 
                    ConnectNodeNeighbours(newNode);

                    nodeList.Add(newNode);
                    canBeCreated = true;
                }
            }

            //TEMPORARY because all special rooms currently have only one version which has one door to the south
            //if we create a special room
            else
            {
                //take an existing room
                FloorNode existingNode = nodeList[Random.Range(0, nodeList.Count)];

                //if the existing room has no room at the direction chosen we create the room there and is not a special room
                if (existingNode.GetRoomType() == FloorNode.roomTypeEnum.regular && existingNode.GetNeighbourNode(FloorNode.directionEnum.north) == null)
                {
                    FloorNode newNode = new FloorNode();

                    newNode.SetRoomType(roomType);

                    //add the new room as a neighbour of the existing room
                    existingNode.SetNeighbourNode(FloorNode.directionEnum.north, newNode);

                    //add the existing room as a neighbour of the new room
                    newNode.SetNeighbourNode(GetOppositeDirection(FloorNode.directionEnum.north), existingNode);

                    (int x, int y) = existingNode.GetCoord();

                    newNode.SetCoord(x, y + 1);

                    nodeList.Add(newNode);
                    canBeCreated = true;
                }
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
        nodeList[0].ActivateRoom(true);

        /*destroy all enemy spawners
        redundant with code in CanvasTransitionScript, probably will be deleted 
        when transition from village to dungeon will be added*/
        GameObject[] enemiesList = GameObject.FindGameObjectsWithTag("EnemySpawn");
        foreach (GameObject enemy in enemiesList)
        {
            Destroy(enemy);
        }        
    }

    public void AddSpecialRooms()
    {
        createRandomNode(FloorNode.roomTypeEnum.itemRoom);
        createRandomNode(FloorNode.roomTypeEnum.sellerRoom);
    }

    public void CreateLinkBetweenNodes(FloorNode a, FloorNode b, FloorNode.directionEnum directionFromAToB)
    {
        if (a.GetNeighbourNode(directionFromAToB) == null && b.GetNeighbourNode(GetOppositeDirection(directionFromAToB)) == null)
        {
            //add the new room as a neighbour of the existing room
            a.SetNeighbourNode(directionFromAToB, b);

            //add the existing room as a neighbour of the new room
            b.SetNeighbourNode(GetOppositeDirection(directionFromAToB), a);
        }
    }

    public void ConnectNodeNeighbours(FloorNode baseNode)
    {
        //look if there are rooms neighbouring the base node, and if there are, connect them to the base node
        foreach (FloorNode node in nodeList)
        {
            FloorNode.directionEnum direction;

            if (node.GetCoord().x == baseNode.GetCoord().x + 1 && node.GetCoord().y == baseNode.GetCoord().y)
            {
                direction = FloorNode.directionEnum.east;
                CreateLinkBetweenNodes(baseNode, node, direction);
            }     
            
            if (node.GetCoord().x == baseNode.GetCoord().x - 1 && node.GetCoord().y == baseNode.GetCoord().y)
            {
                direction = FloorNode.directionEnum.west;
                CreateLinkBetweenNodes(baseNode, node, direction);
            }

            if (node.GetCoord().x == baseNode.GetCoord().x && node.GetCoord().y == baseNode.GetCoord().y + 1)
            {
                direction = FloorNode.directionEnum.north;
                CreateLinkBetweenNodes(baseNode, node, direction);
            }

            if (node.GetCoord().x == baseNode.GetCoord().x && node.GetCoord().y == baseNode.GetCoord().y - 1)
            {
                direction = FloorNode.directionEnum.south;
                CreateLinkBetweenNodes(baseNode, node, direction);
            }
        }
    }
}

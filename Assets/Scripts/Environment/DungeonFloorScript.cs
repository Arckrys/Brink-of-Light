using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFloorScript : MonoBehaviour
{
    private static DungeonFloorScript _instance;
    private List<FloorNode> nodeList;

    private int basicRoomsNumber = 8;
    private int currentNodeIndex;

    private int floorLevel;
    private int dungeonLevel;

    private bool isMapRevealed;

    private void Start()
    {
        isMapRevealed = false;
        nodeList = new List<FloorNode>();
        floorLevel = 1;
        dungeonLevel = 0;

        if(!GameObject.FindGameObjectWithTag("Room"))
            GenerateNewFloor();
    }

    public void GenerateNewFloor()
    {
        foreach (FloorNode room in nodeList)
            room.DestroyRoom();

        MinimapScript.MyInstance.ClearMap();
        nodeList.Clear();
        IncreaseFloorLevel();

        //create a first room
        FloorNode newNode = new FloorNode();
        newNode.SetRoomType(FloorNode.roomTypeEnum.regular);
        newNode.SetCoord(0, 0);
        nodeList.Add(newNode);
        SetCurrentNode(newNode);

        //create the number of rooms desired
        int roomNumber = basicRoomsNumber;
        while (roomNumber > 0)
        {
            CreateRandomNode(FloorNode.roomTypeEnum.regular);
            roomNumber--;
        }

        AddSpecialRooms();

        foreach (FloorNode node in nodeList)
        {
            node.SetRoom(node.GetRoomType());
            node.CreateRoom();
            node.ActivateRoom(false);
        }

        InitializeMap(nodeList[0].GetRoom());

        if (isMapRevealed)
            MinimapScript.MyInstance.ShowFullMap(nodeList);

        else
        {
            MinimapScript.MyInstance.AddRoom(newNode);
        }        

        MinimapScript.MyInstance.UpdateCurrentRoomDisplay(GetCurrentNode());
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

    private void CreateRandomNode(FloorNode.roomTypeEnum roomType)
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
            var randomDirection = (FloorNode.directionEnum)Random.Range(0, 4);

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

            //if the existing room is a regular room and has no room at the direction chosen, we create the room there
            if (existingNode.GetNeighbourNode(randomDirection) == null 
                && !isTileOccupied(x, y) 
                && existingNode.GetRoomType() == FloorNode.roomTypeEnum.regular)
            {
                FloorNode newNode = new FloorNode();

                newNode.SetRoomType(roomType);
                
                //connect the new room with the existing room
                CreateLinkBetweenNodes(existingNode, newNode, randomDirection);                

                newNode.SetCoord(x, y);

                if (existingNode.GetRoomType() == FloorNode.roomTypeEnum.regular)
                {
                    //check if there are rooms existing around the new room and connect them.
                    //This allow the level to have loops and not look like a tree 
                    ConnectNodeNeighbours(newNode);
                }

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
        CreateRandomNode(FloorNode.roomTypeEnum.itemRoom);
        if (floorLevel % 2 == 0)
        {
            CreateRandomNode(FloorNode.roomTypeEnum.sellerRoom);
        }

        if (floorLevel == 2)
            CreateRandomNode(FloorNode.roomTypeEnum.miniBossRoom);
        else if (floorLevel == 4)
            CreateRandomNode(FloorNode.roomTypeEnum.bossRoom);
        else
            CreateRandomNode(FloorNode.roomTypeEnum.exitRoom);
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
        if (baseNode.GetRoomType() == FloorNode.roomTypeEnum.regular)
        {
            //look if there are rooms neighbouring the base node, and if there are, connect them to the base node
            foreach (FloorNode node in nodeList)
            {
                if (node.GetRoomType() == FloorNode.roomTypeEnum.regular)
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
    }

    private void IncreaseFloorLevel()
    {
        floorLevel = (floorLevel % 4) + 1;
        if (floorLevel == 1)
            dungeonLevel++;
        print("Current floor : " + floorLevel);
        print("Current dungeon : " + dungeonLevel);
    }

    //return true if the tile given already has a room
    private bool isTileOccupied(int x, int y)
    {
        foreach(FloorNode node in nodeList)
        {
            if (node.GetCoord() == (x, y))
                return true;
        }

        return false;
    }

    public void ShowFullMap()
    {
        isMapRevealed = true;
        MinimapScript.MyInstance.ShowFullMap(nodeList);
    }
}

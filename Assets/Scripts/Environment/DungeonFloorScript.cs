using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFloorScript : MonoBehaviour
{
    private static DungeonFloorScript _instance;
    private List<FloorNode> nodeList;

    private int basicRoomsNumber = 0;
    private int currentNodeIndex;

    private int floorLevel;
    private int dungeonLevel;

    private bool isMapRevealed;

    private void Start()
    {
        isMapRevealed = false;
        nodeList = new List<FloorNode>();
        floorLevel = 3;
        dungeonLevel = 3;

        //if there is no room in the scene, generate a dungeon floor
        if(!GameObject.FindGameObjectWithTag("Room"))
            GenerateNewFloor();
    }

    public void GenerateNewFloor()
    {
        //destroy previous room prefabs and minimap
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

        //create the number of regular rooms desired
        int roomNumber = basicRoomsNumber;
        while (roomNumber > 0)
        {
            CreateRandomNode(FloorNode.roomTypeEnum.regular);
            roomNumber--;
        }

        //add other rooms after
        AddSpecialRooms();

        //when every nodes are placed and have their coordinates
        //we create each node's room prefab and disable them
        foreach (FloorNode node in nodeList)
        {
            node.SetRoom(node.GetRoomType(), dungeonLevel);
            node.CreateRoom();
            node.ActivateRoom(false);
        }

        //create the first room on the minimap
        InitializeMap(nodeList[0].GetRoom());

        //if the player has the item to reveal the map
        if (isMapRevealed)
            MinimapScript.MyInstance.ShowFullMap(nodeList);

        else
        {
            MinimapScript.MyInstance.AddRoom(newNode, false);
        }

        //update the display of the current room on the minimap
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

            //if the existing room is a regular room, is not occupied, has no room at the direction chosen
            //and does not exceed one of the maximum coordinate, we create the room there
            if (existingNode.GetNeighbourNode(randomDirection) == null
                && !isTileOccupied(x, y)
                && existingNode.GetRoomType() == FloorNode.roomTypeEnum.regular
                && x < 4 && x > -4 && y < 4 && y > -4)
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
        //add an item room every floor
        CreateRandomNode(FloorNode.roomTypeEnum.itemRoom);

        //add a seller room every 2 floors
        if (floorLevel % 2 == 0)
        {
            CreateRandomNode(FloorNode.roomTypeEnum.sellerRoom);
        }

        //add mini boss room at floor number 2
        if (floorLevel == 2)
            CreateRandomNode(FloorNode.roomTypeEnum.miniBossRoom);
        //add a boss room at floor number 4
        else if (floorLevel == 4)
            CreateRandomNode(FloorNode.roomTypeEnum.bossRoom);
        //add an exit room if there is no boss or mini boss
        else
            CreateRandomNode(FloorNode.roomTypeEnum.exitRoom);
    }

    public void CreateLinkBetweenNodes(FloorNode a, FloorNode b, FloorNode.directionEnum directionFromAToB)
    {
        //if both nodes have no neighbour at the chosen direction
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
        //connect rooms only if they are regular rooms
        if (baseNode.GetRoomType() == FloorNode.roomTypeEnum.regular)
        {
            //look if there are rooms neighbouring the base node, and if there are, connect them to the base node
            foreach (FloorNode node in nodeList)
            {
                //connect the regular room only to other regular rooms
                if (node.GetRoomType() == FloorNode.roomTypeEnum.regular)
                {
                    FloorNode.directionEnum direction = FloorNode.directionEnum.north;

                    if (node.GetCoord().x == baseNode.GetCoord().x + 1 && node.GetCoord().y == baseNode.GetCoord().y)
                    {
                        direction = FloorNode.directionEnum.east;
                    }

                    if (node.GetCoord().x == baseNode.GetCoord().x - 1 && node.GetCoord().y == baseNode.GetCoord().y)
                    {
                        direction = FloorNode.directionEnum.west;
                    }

                    if (node.GetCoord().x == baseNode.GetCoord().x && node.GetCoord().y == baseNode.GetCoord().y + 1)
                    {
                        direction = FloorNode.directionEnum.north;
                    }

                    if (node.GetCoord().x == baseNode.GetCoord().x && node.GetCoord().y == baseNode.GetCoord().y - 1)
                    {
                        direction = FloorNode.directionEnum.south;
                        
                    }

                    CreateLinkBetweenNodes(baseNode, node, direction);
                }
            }
        }
    }

    private void IncreaseFloorLevel()
    {
        //floor level goes from 1 to 4
        floorLevel = (floorLevel % 4) + 1;

        //whenever we reach the floor 1, we get to the next dungeon
        if (floorLevel == 1)
        {
            dungeonLevel++;

            //start dungeon's music
            MusicManager.MyInstance.SetCurrentMusic("dungeon" + dungeonLevel);
        }
            
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

    public int GetFloorLevel()
    {
        return floorLevel;
    }

    public int GetDungeonLevel()
    {
        return dungeonLevel;
    }
}

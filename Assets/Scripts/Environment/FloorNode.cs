using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorNode
{
    public enum directionEnum { north, east, south, west };
    public enum roomTypeEnum { regular, itemRoom, sellerRoom, exitRoom, miniBossRoom, bossRoom };

    private FloorNode northNode, eastNode, southNode, westNode;

    //this GameObject stores the room prefab of this node
    private GameObject myRoom;
    private string roomName;
    private roomTypeEnum myType;

    private int myX, myY;

    public DungeonFloorScript DungeonFloorScript
    {
        get => default;
        set
        {
        }
    }

    //keep a FloorNode as neighbour in a specific direction
    public void SetNeighbourNode(directionEnum direction, FloorNode node)
    {
        switch (direction)
        {
            case directionEnum.north:
                northNode = node;
                break;

            case directionEnum.east:
                eastNode = node;
                break;

            case directionEnum.south:
                southNode = node;
                break;

            case directionEnum.west:
                westNode = node;
                break;
        }
    }

    //return the neighbour FloorNode at the direction
    public FloorNode GetNeighbourNode(directionEnum direction)
    {
        switch (direction)
        {
            case directionEnum.north:
                return northNode;

            case directionEnum.east:
                return eastNode;

            case directionEnum.south:
                return southNode;

            case directionEnum.west:
                return westNode;

            default:
                return null;
        }
    }

    //return all FloorNode neighbours of this FloorNode
    public List<FloorNode> GetNeigbours()
    {
        List<FloorNode> neighbours = new List<FloorNode>();
        if (northNode != null)
            neighbours.Add(northNode);
        if (eastNode != null)
            neighbours.Add(eastNode);
        if (southNode != null)
            neighbours.Add(southNode);
        if (westNode != null)
            neighbours.Add(westNode);
        return neighbours;
    }

    //get the correct name of the room based on the room type and neighbours
    public void SetRoom(roomTypeEnum roomType, int dungeonLevel)
    {
        string roomDirections = "";

        //start with the room type
        switch (roomType)
        {
            case roomTypeEnum.itemRoom:
                roomDirections = "ItemRoom";
                break;

            case roomTypeEnum.sellerRoom:
                roomDirections = "SellerRoom";
                break;

            case roomTypeEnum.exitRoom:
                roomDirections = "ExitRoom";
                break;

            case roomTypeEnum.miniBossRoom:
                roomDirections = "MiniBossRoom";
                break;

            case roomTypeEnum.bossRoom:
                roomDirections = "BossRoom";
                break;

            case roomTypeEnum.regular:
                //load the correct prefab based on the present neighbour rooms
                roomDirections = "Room";             
                break;
        }

        //then add neighbours
        if (northNode != null)
            roomDirections += 'N';

        if (eastNode != null)
            roomDirections += 'E';

        if (southNode != null)
            roomDirections += 'S';

        if (westNode != null)
            roomDirections += 'W';

        //load the correct prefab in a GameObject
        myRoom = Resources.Load("Prefabs/Environment/Dungeon " + dungeonLevel + "/Rooms/" + roomDirections) as GameObject;
        roomName = roomDirections;
    }

    public string GetRoomName()
    {
        return roomName;
    }

    public Object GetRoom()
    {
        return myRoom;
    }

    public void SetRoomType(roomTypeEnum type)
    {
        myType = type;
    }

    public roomTypeEnum GetRoomType()
    {
        return myType;
    }

    public void SetCoord(int x, int y)
    {
        myX = x;
        myY = y;
    }

    public (int x, int y) GetCoord()
    {
        return (myX, myY);
    }

    public void CreateRoom()
    {
        myRoom = GameObject.Instantiate(myRoom);
    }

    public void ActivateRoom(bool b)
    {
        myRoom.SetActive(b);
    }

    public void DestroyRoom()
    {
        GameObject.Destroy(myRoom);
    }
}

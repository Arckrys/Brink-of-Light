using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorNode
{
    public enum directionEnum { north, east, south, west };
    public enum roomTypeEnum { regular, itemRoom, sellerRoom };

    private FloorNode northNode, eastNode, southNode, westNode;

    private Object myRoom;
    private string roomName;
    private roomTypeEnum myType;

    private int myX, myY;

    private bool enemiesCleared;

    private GameObject[] combustiblesList;

    public FloorNode()
    {
        enemiesCleared = false;
        combustiblesList = null;
    }

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

    /*
    public string SetRoom()
    {
        //load the correct prefab based on the present neighbour rooms
        string roomDirections = "Room";

        if (northNode != null)
            roomDirections += 'N';

        if (eastNode != null)
            roomDirections += 'E';

        if (southNode != null)
            roomDirections += 'S';

        if (westNode != null)
            roomDirections += 'W';

        myRoom = Resources.Load("Prefabs/Environment/Dungeon 1/Rooms/" + roomDirections);

        roomName = roomDirections;

        return roomDirections;
    }*/

    public void SetRoom(roomTypeEnum roomType)
    {
        switch (roomType)
        {
            case roomTypeEnum.itemRoom:
                myRoom = Resources.Load("Prefabs/Environment/Dungeon 1/Rooms/ItemRoomS");
                break;

            case roomTypeEnum.sellerRoom:
                myRoom = Resources.Load("Prefabs/Environment/Dungeon 1/Rooms/SellerRoomS");
                break;

            case roomTypeEnum.regular:
                //load the correct prefab based on the present neighbour rooms
                string roomDirections = "Room";

                if (northNode != null)
                    roomDirections += 'N';

                if (eastNode != null)
                    roomDirections += 'E';

                if (southNode != null)
                    roomDirections += 'S';

                if (westNode != null)
                    roomDirections += 'W';

                myRoom = Resources.Load("Prefabs/Environment/Dungeon 1/Rooms/" + roomDirections);

                roomName = roomDirections;
                break;
        }
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

    public bool EnemiesCleared
    {
        get
        {
            return enemiesCleared;
        }

        set
        {
            enemiesCleared = value;
        }
    }
}

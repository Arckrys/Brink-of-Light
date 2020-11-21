using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorNode
{
    public enum directionEnum {north, east, south, west}

    private FloorNode northNode, eastNode, southNode, westNode;

    private Object myRoom;
    private string roomName;

    public void SetNeighbourNode(directionEnum direction, FloorNode node)
    {
        switch(direction)
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

    public string SetRoom()
    {
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
    }

    public string GetRoomName()
    {
        return roomName;
    }

    public Object GetRoom()
    {
        return myRoom;
    }
}

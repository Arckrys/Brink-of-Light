﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    private GameObject mapRoom;
    private static MinimapScript _instance;
    [SerializeField] private GameObject panel;

    private List<FloorNode> shownNodes;

    private void Start()
    {
        mapRoom = Resources.Load("Prefabs/UI/MapRoom") as GameObject;
        shownNodes = new List<FloorNode>();
    }

    public static MinimapScript MyInstance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType<MinimapScript>();
            }

            return _instance;
        }
    }

    public void ShowFullMap(List<FloorNode> roomsList)
    {
        foreach (FloorNode node in roomsList)
        {
            if (!shownNodes.Contains(node))
            {
                shownNodes.Add(node);
                GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
                room.transform.position = room.transform.parent.position;
                room.transform.position += new Vector3(node.GetCoord().Item1 * 60, node.GetCoord().Item2 * 20, 0);
            }
        }
    }

    public void AddRoom(FloorNode node)
    {
        if (!shownNodes.Contains(node))
        {
            shownNodes.Add(node);
            GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
            room.transform.position = room.transform.parent.position;
            room.transform.position += new Vector3(node.GetCoord().Item1 * 60, node.GetCoord().Item2 * 20, 0);
        }
        
    }

}

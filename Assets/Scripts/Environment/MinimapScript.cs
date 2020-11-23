using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapScript : MonoBehaviour
{
    private GameObject mapRoom;
    private static MinimapScript _instance;
    [SerializeField] private GameObject panel;

    private List<FloorNode> shownNodes;
    private List<GameObject> displayedRooms;

    private void Start()
    {
        shownNodes = new List<FloorNode>();
        displayedRooms = new List<GameObject>();
        mapRoom = Resources.Load("Prefabs/UI/MapRoom") as GameObject;
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
        if (shownNodes == null)
            this.Start();
        foreach (FloorNode node in roomsList)
        {
            if (!shownNodes.Contains(node))
            {
                shownNodes.Add(node);
                GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
                room.transform.position = room.transform.parent.position;
                room.transform.position += new Vector3(node.GetCoord().x * 60, node.GetCoord().y * 20, 0);
            }
        }
    }

    public void AddRoom(FloorNode node)
    {
        if (shownNodes == null)
            this.Start();

        if (!shownNodes.Contains(node))
        {
            shownNodes.Add(node);
            GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
            room.transform.position = room.transform.parent.position;
            room.transform.position += new Vector3(node.GetCoord().x * 60, node.GetCoord().y * 20, 0);

            displayedRooms.Add(room);
        }
        
    }

    public void UpdateCurrentRoomDisplay(FloorNode currentNode)
    {
        foreach (GameObject roomDisplayed in displayedRooms)
        {
            Color newColor = roomDisplayed.GetComponent<Image>().color;
            newColor.g = 255;
            newColor.b = 255;
            roomDisplayed.GetComponent<Image>().color = newColor;
        }

        GameObject room = displayedRooms[shownNodes.IndexOf(currentNode)];

        Color newColorRed = room.GetComponent<Image>().color;
        newColorRed.g = 0;
        newColorRed.b = 0;
        room.GetComponent<Image>().color = newColorRed;
    }
}

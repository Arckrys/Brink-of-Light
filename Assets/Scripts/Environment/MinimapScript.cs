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

    [SerializeField] private Sprite iconSeller, iconBoss, iconItem, iconExit;

    GameObject roomDisplayed;

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
                CreateRoom(node, false);
            }
        }
    }

    public void AddRoom(FloorNode node, bool isNeighbour)
    {
        if (shownNodes == null)
            this.Start();

        if (!shownNodes.Contains(node))
        {
            if (!isNeighbour)
            {
                DisplayNodeNeighbours(node);
                CreateRoom(node, false);
            }

            else
                CreateRoom(node, true);
        }        
    }

    public void UpdateCurrentRoomDisplay(FloorNode newCurrentNode)
    {
        if (roomDisplayed == null)
            roomDisplayed = displayedRooms[shownNodes.IndexOf(newCurrentNode)];

        else
        {
            Color newColor = roomDisplayed.GetComponent<Image>().color;
            newColor.g = 255;
            newColor.b = 255;
            roomDisplayed.GetComponent<Image>().color = newColor;
        }

        roomDisplayed = displayedRooms[shownNodes.IndexOf(newCurrentNode)];

        SetRoomTransparency(roomDisplayed, 0.6f);
        Color newColorRed = roomDisplayed.GetComponent<Image>().color;
        newColorRed.g = 0;
        newColorRed.b = 0;
        roomDisplayed.GetComponent<Image>().color = newColorRed;
        DisplayNodeNeighbours(newCurrentNode);
    }

    //show the new room's neighbours as semi transparent rooms
    private void DisplayNodeNeighbours(FloorNode node)
    {        
        List<FloorNode> neighbourNodes = node.GetNeigbours();
        foreach (FloorNode neighbour in neighbourNodes)
        {
            AddRoom(neighbour, true);
        }
    }

    public void ClearMap()
    {
        shownNodes.Clear();
        foreach (GameObject room in displayedRooms)
        {
            Destroy(room);
        }
        displayedRooms.Clear();
    }

    private void CreateRoom(FloorNode node, bool isNeighbour)
    {
        //add the node to the list of shown rooms
        shownNodes.Add(node);
        
        //create a room on the minimap
        GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
        room.transform.position = room.transform.parent.position;
        room.transform.position += new Vector3(node.GetCoord().x * 60, node.GetCoord().y * 20, 0);

        //handle the icon of specific rooms
        FloorNode.roomTypeEnum roomType = node.GetRoomType();

        switch (roomType)
        {
            case FloorNode.roomTypeEnum.sellerRoom:                
                room.transform.GetChild(0).GetComponent<Image>().sprite = iconSeller;
                break;

            case FloorNode.roomTypeEnum.bossRoom:
            case FloorNode.roomTypeEnum.miniBossRoom:
                room.transform.GetChild(0).GetComponent<Image>().sprite = iconBoss;
                break;

            case FloorNode.roomTypeEnum.exitRoom:
                room.transform.GetChild(0).GetComponent<Image>().sprite = iconExit;
                break;

            case FloorNode.roomTypeEnum.itemRoom:
                room.transform.GetChild(0).GetComponent<Image>().sprite = iconItem;
                break;

            default:
                Destroy(room.transform.GetChild(0).gameObject);
                break;
        }

        displayedRooms.Add(room);

        if (isNeighbour)
            SetRoomTransparency(room, 0.3f);
    }

    private void SetRoomTransparency(GameObject room, float transparencyValue)
    {
        Color newColor = room.GetComponent<Image>().color;
        newColor.a = transparencyValue;
        room.GetComponent<Image>().color = newColor;
    }
}

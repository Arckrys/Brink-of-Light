using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour
{
    GameObject mapRoom;
    private static MinimapScript _instance;
    [SerializeField] public GameObject panel;

    private void Start()
    {
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

    public void InitializeMiniMap(List<(int, int)> roomsCoords)
    {
        foreach((int, int) coord in roomsCoords)
        {
            GameObject room = GameObject.Instantiate(mapRoom, panel.transform);
            room.transform.position = room.transform.parent.position;
            room.transform.position += new Vector3(coord.Item1 * 60, coord.Item2 * 20, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTransitionScript : MonoBehaviour
{
    [SerializeField] private float transitionSpeed;

    public IEnumerator FadeIn(GameObject exitDoor)
    {
        var canvasGroup = GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>();
        while (canvasGroup.alpha < 1)
        {
            yield return new WaitForSeconds(transitionSpeed);
            canvasGroup.alpha += 0.1f;
        }

        //set the current room inactive
        DungeonFloorScript.MyInstance.GetCurrentNode().ActivateRoom(false);

        //get the next room direction
        FloorNode.directionEnum direction = FloorNode.directionEnum.north;
        string spawnPointName = "SpawnDown";

        if (exitDoor.CompareTag("DoorRight"))
        {
            direction = FloorNode.directionEnum.east;
            spawnPointName = "SpawnLeft";
        }
        if (exitDoor.CompareTag("DoorDown"))
        {
            direction = FloorNode.directionEnum.south;
            spawnPointName = "SpawnUp";
        }
        if (exitDoor.CompareTag("DoorLeft"))
        {
            direction = FloorNode.directionEnum.west;
            spawnPointName = "SpawnRight";
        }

        print("current room : " + DungeonFloorScript.MyInstance.GetCurrentNode().GetRoomName());
        print("next room : " + DungeonFloorScript.MyInstance.GetCurrentNode().GetNeighbourNode(direction).GetRoomName());

        //get the next room node
        FloorNode nextNode = DungeonFloorScript.MyInstance.GetCurrentNode().GetNeighbourNode(direction);

        //change the current room node as the next room node we picked
        DungeonFloorScript.MyInstance.SetCurrentNode(nextNode);

        //activate the room
        DungeonFloorScript.MyInstance.GetCurrentNode().ActivateRoom(true);

        //reposition the player to the correct place of the room where he should spawn
        UpdatePlayerPosition(spawnPointName);

        //reset the room doors to get the doors of the new room
        GameManager.MyInstance.ResetRoomDoors();

        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        var canvasGroup = GameObject.Find("CanvasTransition").GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            yield return new WaitForSeconds(transitionSpeed);
            canvasGroup.alpha -= 0.1f;
        }
    }

    public void UpdatePlayerPosition(string spawnPointName)
    {
        PlayerScript.MyInstance.transform.position = GameObject.Find(spawnPointName).transform.position;
    }
}

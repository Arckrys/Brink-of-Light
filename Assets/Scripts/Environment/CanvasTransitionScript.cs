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

        //destroy the current room - we will need to store it before destroying it to keep the state of the room if the player come back in it later
        GameObject oldRoom = GameObject.FindGameObjectWithTag("Room");
        Destroy(oldRoom);

        //get the next room direction
        FloorNode.directionEnum direction = FloorNode.directionEnum.north;
        string spawnPointName = "SpawnDown";

        if (exitDoor.tag == "DoorRight")
        {
            direction = FloorNode.directionEnum.east;
            spawnPointName = "SpawnLeft";
        }
        if (exitDoor.tag == "DoorDown")
        {
            direction = FloorNode.directionEnum.south;
            spawnPointName = "SpawnUp";
        }
        if (exitDoor.tag == "DoorLeft")
        {
            direction = FloorNode.directionEnum.west;
            spawnPointName = "SpawnRight";
        }

        print("current room : " + DungeonFloorScript.MyInstance.GetCurrentNode().GetRoomName());
        print("next room : " + DungeonFloorScript.MyInstance.GetCurrentNode().GetNeighbourNode(direction).GetRoomName());

        FloorNode nextNode = DungeonFloorScript.MyInstance.GetCurrentNode().GetNeighbourNode(direction);

        Object nextRoom = nextNode.GetRoom();

        DungeonFloorScript.MyInstance.SetCurrentNode(nextNode);

        //instantiate the next room
        Instantiate(nextRoom);

        print(nextNode.GetCoord());

        //destroy the enemy spawners if the room has been cleared by the player before
        if (nextNode.EnemiesCleared)
        {
            GameObject[] enemiesList = GameObject.FindGameObjectsWithTag("EnemySpawn");
            foreach (GameObject enemy in enemiesList)
            {
                Destroy(enemy);
            }
        }

        //reposition the player to the correct place of the room where he should spawn
        UpdatePlayerPosition(spawnPointName);

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

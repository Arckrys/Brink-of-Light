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

        //reposition the player to the correct place of the room where he should spawn
        exitDoor.GetComponent<ExitManager>().UpdatePlayerPosition();

        //destroy the current room - we will need to store it before destroying it to keep the state of the room if the player come back in it later
        Destroy(GameObject.FindGameObjectWithTag("Room"));

        //instantiate the next room
        Instantiate(Resources.Load("Prefabs/Environment/Dungeon 1/Rooms/RoomS"));



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
}

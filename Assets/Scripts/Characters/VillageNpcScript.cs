using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VillageNpcScript : MonoBehaviour
{
    [SerializeField] protected GameObject menuGameObject;

    protected bool isPlayerInRange = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInRange = true;

            gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInRange = false;

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}

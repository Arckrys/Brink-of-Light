using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopedieScript : MonoBehaviour
{

    [SerializeField] private GameObject menuEncyclopedie;
    [SerializeField] private GameObject itemsScroll;
    [SerializeField] private GameObject mobsScroll;

    private bool isItemShown = true;
    private bool isInCollision = false;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || !isInCollision) return;

        menuEncyclopedie.SetActive(!menuEncyclopedie.activeSelf);
        PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        isInCollision = true;

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        isInCollision = false;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }


    /* Affiche le canvas des items (bouton) */
    public void showItems()
    {
        if (!isItemShown)
        {
            isItemShown = true;
            itemsScroll.SetActive(true);
            mobsScroll.SetActive(false);
        }
    }

    /* Affiche le canvas des ennemis (bouton) */
    public void showMobs()
    {
        if (isItemShown)
        {
            isItemShown = false;
            itemsScroll.SetActive(false);
            mobsScroll.SetActive(true);
        }
    }
}

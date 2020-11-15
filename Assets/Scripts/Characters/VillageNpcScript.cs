using UnityEngine;

public enum NPCName {Razakus, Igeirus, Urbius}

public abstract class VillageNpcScript : MonoBehaviour
{
    [SerializeField] protected GameObject menuGameObject;

    protected NPCName? playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        if (gameObject.CompareTag(NPCName.Razakus.ToString()))
        {
            playerInRange = NPCName.Razakus;
        }
        else if (gameObject.CompareTag(NPCName.Igeirus.ToString()))
        {
            playerInRange = NPCName.Igeirus;
        }
        else if (gameObject.CompareTag(NPCName.Urbius.ToString()))
        {
            playerInRange = NPCName.Urbius;
        }

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        playerInRange = null;

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
}

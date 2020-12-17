using UnityEngine;

public enum NPCName {Razakus, Igeirus, Urbius, Talker}

public class VillageNpcScript : MonoBehaviour
{
    [SerializeField] protected GameObject menuGameObject;

    [SerializeField] private NPCName sellerName;

    [SerializeField] protected bool isSeller;

    protected NPCName SellerName => sellerName;

    protected bool isInCollision;

    // Display an NPC's name above their head when the player is near
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
}

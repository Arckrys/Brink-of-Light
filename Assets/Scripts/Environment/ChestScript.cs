using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private bool isOpen;
    private Animator chestAnimator;

    [SerializeField] private GameObject lootBag;

    private void Start()
    {
        chestAnimator = GetComponent<Animator>();
        isOpen = false;

        if (GameObject.Find("ChestSpawnPoint"))
            transform.position = GameObject.Find("ChestSpawnPoint").transform.position;
    }

    private void Update()
    {
        chestAnimator.SetBool("isOpened", isOpen);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            isOpen = true;

            //create 3 bags
            string randomConsumableItem = null;
            int consumableItemProbability = Random.Range(0, 25);

            var itemManager = ItemsManagerScript.MyInstance;

            if (consumableItemProbability == 0)
                randomConsumableItem = itemManager.SelectRandomItem(itemManager.ConsumableItems);

            for (int i = 0; i < 3; i++)
            {
                var loot = Instantiate(lootBag, new Vector2(transform.position.x - 1 + i, transform.position.y - 1), Quaternion.identity);
                loot.transform.parent = this.transform.parent;
                loot.GetComponent<LootManager>().CreateBag(randomConsumableItem, 0, Random.Range(0, 3), Random.Range(0, 6));
            }

            //has a small probability to spawn an equipment item
            int equipmentItemProbability = Random.Range(0, 4);

            if (equipmentItemProbability == 0)
            {
                string randomEquipmentItem = itemManager.SelectRandomItem(itemManager.EquipmentItems);
                var item = itemManager.CreateEquipmentItem(new Vector2(transform.position.x, transform.position.y + 1), randomEquipmentItem);
                item.transform.parent = this.transform.parent;
            }
        }
    }
}

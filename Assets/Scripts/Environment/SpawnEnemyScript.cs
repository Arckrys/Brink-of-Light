using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyScript : MonoBehaviour
{

    private int dungeon;
    private int floor;

    private GameObject[] enemyArray;

    // Start is called before the first frame update
    void Start()
    {
        dungeon = DungeonFloorScript.MyInstance.GetDungeonLevel();
        floor = DungeonFloorScript.MyInstance.GetFloorLevel();

        string prefabsPath = "Prefabs/Enemies/Dungeon" + dungeon;
        enemyArray = Resources.LoadAll<GameObject>(prefabsPath);

        //get the sum of all enemies probability
        int probabilitySum = 0;
        foreach (GameObject o in enemyArray)
        {
            int enemyProbability = o.GetComponent<BasicEnemyController>().GetSpawnProbabilities()[floor - 1];

            //keep only the enemies with a spawn probability higher than zero
            if (enemyProbability > 0)
            {
                probabilitySum += enemyProbability;
            }
        }

        int randomInt = Random.Range(0, probabilitySum);

        probabilitySum = 0;

        foreach (GameObject o in enemyArray)
        {
            int enemyProbability = o.GetComponent<BasicEnemyController>().GetSpawnProbabilities()[floor - 1];

            if (enemyProbability > 0)
            {
                probabilitySum += enemyProbability;

                if (randomInt < probabilitySum)
                {
                    GameObject enemy = Instantiate(o, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    enemy.transform.parent = this.gameObject.transform.parent;
                    break;
                }
            }
        }
    }
}

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

        //get all the enemy prefabs of the current dungeon
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

        //get a random number in the range of the sum of spawn probabilites
        int randomInt = Random.Range(0, probabilitySum);

        probabilitySum = 0;

        //we go through the array of spawnable enemies, adding their probabilities to a new sum each time.
        foreach (GameObject o in enemyArray)
        {
            int enemyProbability = o.GetComponent<BasicEnemyController>().GetSpawnProbabilities()[floor - 1];

            if (enemyProbability > 0)
            {
                probabilitySum += enemyProbability;

                //if the random number is inferior to the sum of probabilites, then spawn the current enemy of the foreach
                //and attach it to the room
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyScript : MonoBehaviour
{

    [SerializeField] private int dungeon;
    [SerializeField] private int level;

    private GameObject[] enemyArray;

    // Start is called before the first frame update
    void Start()
    {
        string prefabsPath = "Prefabs/Enemies/Dungeon" + dungeon;
        enemyArray = Resources.LoadAll<GameObject>(prefabsPath);

        //get the sum of all enemies probability
        int probabilitySum = 0;
        foreach (GameObject o in enemyArray)
        {
            int enemyProbability = o.GetComponent<BasicEnemyController>().GetSpawnProbabilities()[level - 1];

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
            int enemyProbability = o.GetComponent<BasicEnemyController>().GetSpawnProbabilities()[level - 1];

            if (enemyProbability > 0)
            {
                probabilitySum += enemyProbability;

                if (randomInt < probabilitySum)
                {
                    Instantiate(o, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
                    break;
                }
            }
        }
    }
}

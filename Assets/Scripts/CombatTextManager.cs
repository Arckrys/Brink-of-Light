using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField] private GameObject combatTextPrefab;

    public void CreateText(Vector2 position, string text)
    {
        Text temp = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        temp.transform.position = position;
    }
}

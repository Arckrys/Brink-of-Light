using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType {DAMAGE, HEAL}

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

    public void CreateText(Vector2 position, string text, DamageType type)
    {
        position.y += 1.0f;

        Text temp = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        temp.transform.position = position;

        string operation = string.Empty;

        switch (type)
        {
            case DamageType.DAMAGE:
                operation += "-";
                temp.color = Color.red;
                break;
            case DamageType.HEAL:
                operation += "+";
                temp.color = Color.green;
                break;
        }

        temp.text = operation + text;
    }
}

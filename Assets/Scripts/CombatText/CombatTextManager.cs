using UnityEngine;
using UnityEngine.UI;

public enum DamageType {Damage, Heal, Player, DamageOnTime}

public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager _instance;

    public static CombatTextManager MyInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CombatTextManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject combatTextPrefab;
    
    private static readonly int Crit = Animator.StringToHash("crit");
    
    // Creates the combat text to display above the character
    public void CreateText(Vector2 position, string text, DamageType type, float offset, bool crit)
    {
        position.y += offset;

        var temp = Instantiate(combatTextPrefab, transform).GetComponent<Text>();
        temp.transform.position = position;

        var operation = string.Empty;

        // According to the type of damage, change tex color
        switch (type)
        {
            case DamageType.Damage:
                operation += "-";
                temp.color = Color.red;
                break;
            case DamageType.Heal:
                operation += "+";
                temp.color = Color.green;
                break;
            case DamageType.Player:
                operation += "-";
                temp.color = Color.blue;
                break;
            case DamageType.DamageOnTime:
                operation += "-";
                Color orangeColor = new Color(1.0f, 0.64f, 0.0f);
                temp.color = orangeColor;
                break;
            default:
                temp.color = Color.white;
                break;
        }

        temp.text = operation + text;

        if (!crit) return;
        
        temp.color = Color.yellow;

        // Plays zooming animation for critic damage
        temp.GetComponent<Animator>().SetBool(Crit, true);
    }
}

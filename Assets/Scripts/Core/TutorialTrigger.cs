using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType { Ennemy, Equipment, Consumable, Combustible }

public class TutorialTrigger : MonoBehaviour
{
    
    [SerializeField] private TriggerType triggerType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (triggerType)
        {
            case TriggerType.Ennemy:
                {
                    if (other.gameObject.CompareTag("Spell"))
                        TutorialScript.MyInstance.TriggerEnnemy();
                    break;
                }
            case TriggerType.Equipment:
                {
                    if (other.gameObject.CompareTag("Player"))
                        TutorialScript.MyInstance.TriggerEquipment();
                    break;
                }
            case TriggerType.Consumable:
                {
                    if (other.gameObject.CompareTag("Player"))
                        TutorialScript.MyInstance.TriggerConsumable();
                    break;
                }
            case TriggerType.Combustible:
                {
                    if (other.gameObject.CompareTag("Player"))
                        TutorialScript.MyInstance.TriggerCombustible();
                    break;
                }
            default: break;
        }
    }


}

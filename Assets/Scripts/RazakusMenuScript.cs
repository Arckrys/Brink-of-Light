using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazakusMenuScript : MonoBehaviour
{

    public void Start()
    {

    }

    public void OnHealthPressed()
    {
        PlayerScript.MyInstance.LifeMaxValue += 10f;
    }
    public void OnAttackPressed()
    {
        Debug.Log("Attack pressed");
        PlayerScript.MyInstance.AttackMaxValue += 10f;
    }
    public void OnRangePressed()
    {
        Debug.Log("Range pressed");
        PlayerScript.MyInstance.RangeMaxValue += 10f;
    }
    public void OnCritChancePressed()
    {
        Debug.Log("Crit pressed");
        PlayerScript.MyInstance.CritChanceMaxValue += 10f;
    }
    public void OnSpeedPressed()
    {
        Debug.Log("Speed pressed");
        PlayerScript.MyInstance.MovementSpeedMaxValue += 10f;
    }


}

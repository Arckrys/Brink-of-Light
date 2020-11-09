using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazakusNpcScript : VillageNpcScript
{
    private RazakusMenuScript razakusScript;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInRange)
        {
            bool isMenuActivated = menuGameObject.activeSelf;

            menuGameObject.SetActive(!menuGameObject.activeSelf);

            if (!isMenuActivated)
            {
                razakusScript = RazakusMenuScript.MyInstance;
                razakusScript.InitUI();
            }

            PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());
        }
    }
}

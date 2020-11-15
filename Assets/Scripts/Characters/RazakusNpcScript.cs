using System;
using UnityEngine;

public class RazakusNpcScript : VillageNpcScript
{
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || playerInRange == null) return;
        
        var isMenuActivated = menuGameObject.activeSelf;

        menuGameObject.SetActive(!menuGameObject.activeSelf);

        if (!isMenuActivated && playerInRange != null)
        {
            switch (playerInRange)
            {
                case NPCName.Razakus:
                    RazakusMenuScript.MyInstance.InitUI();
                    break;
                case NPCName.Igeirus:
                case NPCName.Urbius:
                {
                    var seller = SellerMenuScript.MyInstance;
                    seller.MyName = (NPCName) playerInRange;
                    seller.UpdateUI();
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());
    }
}

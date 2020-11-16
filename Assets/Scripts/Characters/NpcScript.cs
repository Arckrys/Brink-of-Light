using System;
using UnityEngine;

public class NpcScript : VillageNpcScript
{
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || !isInCollision) return;

        var isMenuActivated = menuGameObject.activeSelf;

        menuGameObject.SetActive(!menuGameObject.activeSelf);

        if (!isMenuActivated)
        {
            switch (SellerName)
            {
                case NPCName.Razakus:
                    RazakusMenuScript.MyInstance.InitUI();
                    break;
                case NPCName.Igeirus:
                case NPCName.Urbius:
                {
                    var seller = SellerMenuScript.MyInstance;
                    seller.MySellerName = SellerName;
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

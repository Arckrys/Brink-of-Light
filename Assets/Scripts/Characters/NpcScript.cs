using System;
using UnityEngine;

public class NpcScript : VillageNpcScript
{ 
    private GameManager game;

    private void Start()
    {
        game = GameManager.MyInstance;
    }
    
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
                    //game.LoadGame();
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
        else
        {
            switch (SellerName)
            {
                case NPCName.Razakus:
                    //game.SaveGame();
                    break;
                case NPCName.Igeirus:
                    break;
                case NPCName.Urbius:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());
    }
}

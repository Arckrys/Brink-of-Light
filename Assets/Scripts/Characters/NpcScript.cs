using System;
using UnityEngine;

public class NpcScript : VillageNpcScript
{
    private GameManager game;
    private SellerMenuScript seller;

    [SerializeField] private Dialogue dialogue;

    private bool sellerDialogueDone = false;
    private bool isMenuActivated = false;

    private void Start()
    {
        game = GameManager.MyInstance;
        seller = SellerMenuScript.MyInstance;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) || !isInCollision) return;

        if (isSeller & sellerDialogueDone)
        {
            isMenuActivated = menuGameObject.activeSelf;
            menuGameObject.SetActive(!menuGameObject.activeSelf);
            PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());
        }

        if (!isMenuActivated)
        {
            switch (SellerName)
            {
                case NPCName.Razakus:
                    RazakusMenuScript.MyInstance.InitUI();
                    if(!sellerDialogueDone)
                        TriggerDialogue();
                    break;
                case NPCName.Igeirus:
                case NPCName.Urbius:
                    {
                        seller.MySellerName = SellerName;
                        seller.UpdateUI();
                        if (!sellerDialogueDone)
                            TriggerDialogue();
                        break;
                    }
                case NPCName.Talker:
                    {
                        if (!DialogueManagerScript.MyInstance.DialogueIsOpen)
                            TriggerDialogue();
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException();
            }
            sellerDialogueDone = true;
        }

    }
    public void TriggerDialogue()
    {
        DialogueManagerScript.MyInstance.StartDialogue(dialogue);
    }

}
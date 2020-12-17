using System;
using UnityEngine;

public class NpcScript : VillageNpcScript
{
    private SellerMenuScript seller;

    [SerializeField] private Dialogue dialogue;

    private bool sellerDialogueDone = false;
    private bool isMenuActivated = false;

    private void Start()
    {
        seller = menuGameObject.GetComponent<SellerMenuScript>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E) && (!Input.GetKeyDown(KeyCode.Escape) || !PlayerScript.MyInstance.GetIsInMenu()) || !isInCollision) return;

        // Activates the seller's menu
        if (isSeller && sellerDialogueDone && DialogueManagerScript.MyInstance.SentenceIsOver)
        {
            isMenuActivated = menuGameObject.activeSelf;
            menuGameObject.SetActive(!menuGameObject.activeSelf);
            PlayerScript.MyInstance.SetIsInMenu(!PlayerScript.MyInstance.GetIsInMenu());
        }

        // Triggers the NPC's dialogue and updates seller's menu
        if (!isMenuActivated)
        {
            switch (SellerName)
            {
                case NPCName.Razakus:
                    RazakusMenuScript.MyInstance.InitUI();
                    if (!sellerDialogueDone)
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipUpgradesUi : MonoBehaviour
{
    //what player this UI is linked to 
    private string playerLink;
    [SerializeField]
    private GameObject[] deckSlots;

    [SerializeField]
    private TextMeshProUGUI upgradeSlot1Text, upgradeSlot2Text, upgradeSlot3Text;

    private int upgradeSlotSelected = -1;
    void Start()
    {
        foreach (UpgradeSlot slot in FindObjectsOfType<UpgradeSlot>())
        {
            slot.upgradeClick += upgradeClickHandler;
        }

        if (gameObject.name == "Player2 equip Upgrade")
        {
            playerLink = "player2";
        }
        else if (gameObject.name == "Player1 equip Upgrades")
        {
            playerLink = "player1";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateupgradeSlot(string upgradeName, string playerID)
    {
        if (playerID != playerLink)
        {
            return;
        }
            switch (upgradeSlotSelected)
            {
                case 0:
                    //update 0's text
                    upgradeSlot1Text.text = upgradeName;
                    break;
                case 1:
                //update 1's text
                upgradeSlot2Text.text = upgradeName;
                    break;
                case 2:
                //update 2's text
                upgradeSlot3Text.text = upgradeName;
                    break;
                default:
                    //do nothing
                    break;

            }
    }

    public void Show()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void upgradeClickHandler(GameObject sender)
    {
        if (((sender.name == "Player1 Slot 1" || sender.name == "Player 1 Slot 2" || sender.name == "Player 1 Slot 3") && playerLink != "player1") || (sender.name == "Player2 Slot 1" || sender.name == "Player2 Slot 2" || sender.name == "Player2 Slot 3") && playerLink != "player2")
        {
            return;
        }

        upgradeSlotSelected = sender.name[sender.name.Length - 1] - 49;

        //clear deck and redraw
        for (int i = 0; i < deckSlots.Length; i ++){
            deckSlots[i].GetComponent<Image>().enabled = false;
            FindObjectOfType<UpgradeSlot>().floodHideShow(deckSlots[i], false);
        }
        if (playerLink == "player1")
        {
            //player 1
            for (int i = 0; i < FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().deck.Count; i ++)
            {
                deckSlots[i].GetComponent<Image>().enabled = true;
                FindObjectOfType<UpgradeSlot>().floodHideShow(deckSlots[i], true);
                deckSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().deck[i].Name;
            }
            return;
        }
        else if (playerLink == "player2")
        {
            //player 2
            for (int i = 0; i < FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().deck.Count; i++)
            {
                deckSlots[i].GetComponent<Image>().enabled = true;
                FindObjectOfType<UpgradeSlot>().floodHideShow(deckSlots[i], true);
                deckSlots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().deck[i].Name;
            }
        }
    }
}

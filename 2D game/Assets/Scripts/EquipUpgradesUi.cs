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

    public void Show()
    {
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void upgradeClickHandler()
    {
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

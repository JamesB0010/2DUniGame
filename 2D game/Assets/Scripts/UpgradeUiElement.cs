using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeUiElement : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject backPannel;

    [SerializeField]
    private EquipUpgradesUi equipUpgradesPlayer1, equipUpgradesPlayer2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        backPannel.GetComponent<Image>().enabled = false;
        FindObjectOfType<UpgradeSlot>().floodHideShow(backPannel, false);
        if (gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().addUpgrade(int.Parse(gameObject.name));
            equipUpgradesPlayer1.updateupgradeSlot(FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().getUpgrade(int.Parse(gameObject.name)).Name, "player1");

        }
        else if (gameObject.CompareTag("Player2"))
        {
            FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().addUpgrade(int.Parse(gameObject.name));
            equipUpgradesPlayer2.updateupgradeSlot(FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().getUpgrade(int.Parse(gameObject.name)).Name, "player2");
        }
    }
}

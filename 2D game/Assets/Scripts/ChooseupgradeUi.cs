using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChooseupgradeUi : MonoBehaviour
{

    public enum redBlueCards{
        card1Player1,
        card1Player2,
        card2Player1,
        card2Player2,
        card3Player1,
        card3Player2
    };

    [SerializeField]
    GameObject[] cardTexts;

    [SerializeField]
    //the parent game object for the upgrade cards this allows cards to be enabled / disabled depending on how many upgrades the player has to choose from
    private GameObject[] cardGOs;

    private Upgrade[] cards = new Upgrade[3];

    //the red and blue boxes
    [SerializeField]
    private GameObject[] visualPlayerSelectors;

    private int player1Selection;
    [SerializeField]
    private Image currentActiveVisualCardP1;
    private int player2Selection;
    [SerializeField]
    private Image currentActiveVisualCardP2;


    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().OnRaceOver += onRaceOver;

        StartCoroutine(generateRandomCards());
    }

    public IEnumerator generateRandomCards()
    {
        yield return null;
        for (int i = 0; i < 3; i++)
        {
            Upgrade card = FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().randomUpgrade();
            if (card != null)
            {
                cards[i] = card;
            }
        }
        setCardText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onRaceOver()
    {
        StartCoroutine("helloGUI");
    }

    public IEnumerator helloGUI()
    {
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Canvas>().enabled = true;
        foreach(Hud hud in FindObjectsOfType<Hud>())
        {
            hud.gameObject.GetComponent<Canvas>().enabled = false;
        }
    }

    public void continueButtonPressed()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        foreach(EquipUpgradesUi UI in FindObjectsOfType<EquipUpgradesUi>())
        {
            UI.Show();
        }

        //add the players selections to their decks
        FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().deck.Add(cards[player1Selection]);
        FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().deck.Add(cards[player2Selection]);

        //remove the selection from player 1 and player 2's valid list
        FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().removeNameFromValidList(FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().deck[player1Selection].Name);
        FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().removeNameFromValidList(FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().deck[player2Selection].Name);
        FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().removeNameFromValidList(FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().deck[player1Selection].Name);
        FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().removeNameFromValidList(FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().deck[player2Selection].Name);
    }

    private void setCardText()
    {
        //Debug.Log(cards.Length);
        for (int i = 0; i < cardTexts.Length; i = i + 2)
        {
            cardTexts[i].GetComponent<TextMeshProUGUI>().enabled = false;
            cardTexts[i + 1].GetComponent<TextMeshProUGUI>().enabled = false;

            // set active method got from api https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
            cardGOs[i / 2].gameObject.SetActive(false);

            if (cards[i / 2] != null)
            {
                cardTexts[i].GetComponent<TextMeshProUGUI>().text = cards[i / 2].Name;
                cardTexts[i + 1].GetComponent<TextMeshProUGUI>().text = cards[i / 2].Description;

                cardTexts[i].GetComponent<TextMeshProUGUI>().enabled = true;
                cardTexts[i + 1].GetComponent<TextMeshProUGUI>().enabled = true;

                cardGOs[i / 2].SetActive(true);
            }
        }

    }

    public void playerSelection(int cardNumber)
    {
        switch (cardNumber)
        {
            case 0:
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card1Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card1Player1].GetComponent<Image>();
                player1Selection = 0;
                break;
            case 1:
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>();
                player2Selection = 0;
                break;
            case 2:
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>();
                player1Selection =1;
                break;
            case 3:
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>();
                player2Selection = 1;
                break;
            case 4:
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>();
                player1Selection = 2;
                break;
            case 5:
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>();
                player2Selection = 2;
                break;
        }
    }

}

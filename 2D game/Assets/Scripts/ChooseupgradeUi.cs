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
            cards[i] = FindObjectOfType<PlayerUpgrades>().randomUpgrade();
        }
        for (int i = 0; i < 3; i++)
        {
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
    }

    private void setCardText()
    {
        for(int i = 0; i < cardTexts.Length; i = i + 2)
        {
            cardTexts[i].GetComponent<TextMeshProUGUI>().text = cards[i / 2].Name;
            cardTexts[i + 1].GetComponent<TextMeshProUGUI>().text = cards[i / 2].Description;
        }
    }

    public void playerSelection(int cardNumber)
    {
        switch (cardNumber)
        {
            case 0:
                Debug.Log("Card1 player 1");
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card1Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card1Player1].GetComponent<Image>();
                player1Selection = 0;
                break;
            case 1:
                Debug.Log("Card1 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>();
                player2Selection = 0;
                break;
            case 2:
                Debug.Log("Card2 Player 1");
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>();
                player1Selection =1;
                break;
            case 3:
                Debug.Log("Card2 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>();
                player2Selection = 1;
                break;
            case 4:
                Debug.Log("Card3 Player 1");
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>();
                player1Selection = 2;
                break;
            case 5:
                Debug.Log("Card3 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>();
                player2Selection = 2;
                break;
        }
    }

}

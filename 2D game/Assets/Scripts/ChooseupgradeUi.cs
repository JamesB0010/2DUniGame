using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                break;
            case 1:
                Debug.Log("Card1 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card1Player2].GetComponent<Image>();
                break;
            case 2:
                Debug.Log("Card2 Player 1");
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card2Player1].GetComponent<Image>();
                break;
            case 3:
                Debug.Log("Card2 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card2Player2].GetComponent<Image>();
                break;
            case 4:
                Debug.Log("Card3 Player 1");
                currentActiveVisualCardP1.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP1 = visualPlayerSelectors[(int)redBlueCards.card3Player1].GetComponent<Image>();
                break;
            case 5:
                Debug.Log("Card3 Player 2");
                currentActiveVisualCardP2.enabled = false;
                visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>().enabled = true;
                currentActiveVisualCardP2 = visualPlayerSelectors[(int)redBlueCards.card3Player2].GetComponent<Image>();
                break;
        }
    }

}

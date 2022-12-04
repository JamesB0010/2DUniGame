using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSaver : MonoBehaviour
{

    private GameObject instance;
    private struct PlayerData{
        public List<Upgrade> valid;
        public List<Upgrade> deck;
        public List<Upgrade> upgrades;
    }

    PlayerData player1Data;

    PlayerData player2Data;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        saveUpgrades();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player1Data = new PlayerData();
        player2Data = new PlayerData();
        
    }


    public void saveUpgrades()
    {
        player1Data.valid = FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().getValid();
        player1Data.deck = FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().getDeck();
        player1Data.upgrades = FindObjectOfType<Player>().GetComponent<PlayerUpgrades>().getUpgrades();

        player2Data.valid = FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().getValid();
        player2Data.deck = FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().getDeck();
        player2Data.upgrades = FindObjectOfType<Player2>().GetComponent<PlayerUpgrades>().getUpgrades();
    }

    public List<Upgrade>[] loadData(string Player)
    {
        if (Player == "Player")
        {
            return new List<Upgrade>[] {
                player1Data.valid,
                player1Data.deck,
                player1Data.upgrades
        };
        }
        else if (Player == "Player2")
        {
            return new List<Upgrade>[] {
                player2Data.valid,
                player2Data.deck,
                player2Data.upgrades
        };
        }
        return new List<Upgrade>[1];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//https://stackoverflow.com/questions/3767942/storing-a-list-of-methods-in-c-sharp use of using Actions to store a list/array of methods
public class PlayerUpgrades : MonoBehaviour
{
    //makes it easy to reference an action fron the upgrade list
    public enum upgradeMap
    {
        insulatedElectrics,
        sealedCircuits,
        planted,
        Radio
    }


    private List<Upgrade> valid = new List<Upgrade>();

    //holds all the upgrades
    private Upgrade[] upgradeList = new Upgrade[]
    {
        new Upgrade("Insulated Electronics", "Immune to EMP drone", delegate
        {
            InsElec();
        }),

        new Upgrade("Sealed Circuits", "Immune to sparks", delegate
        {
            SeaCirc();
        }),

        new Upgrade("Planted", "Immune to wind turbines", delegate
        {
            Planted();
        }),

        new Upgrade("Radio", "Gain boost when advancing checkpoints", delegate
        {
            Radio();
        }),

        new Upgrade("Combustion engine", "Immune to Coast zones", delegate
        {
            ComEngi();
        })
    };

    public List<Upgrade> deck = new List<Upgrade>();

    public Upgrade randomUpgrade()
    {
        int number = Random.Range(0, valid.Count);
        Upgrade upgrade = valid[number];
        valid.RemoveAt(number);
        return upgrade;
    }

    //holds the equipped upgrades
    private List<Upgrade> upgrades = new List<Upgrade>();

   public Upgrade getUpgrade(int index)
    {
        return upgrades[index];
    }

    public void addUpgrade(int deckIndex)
    {
        upgrades.Add(deck[deckIndex]);
    }

    public int getUpgradesLength()
    {
        return upgrades.Count;
    }

    public void addUpgrade(upgradeMap upgradeName)
    {
        if (upgrades.Count == 3)
        {
            Debug.Log("Already too many upgrades");
            return;
        }

        upgrades.Add(upgradeList[(int)upgradeName]);
    }

    private static void InsElec()
    {
        Debug.Log("insulated electronics");
    }

    private static void SeaCirc()
    {
        Debug.Log("Sealed electronics");
    }

    private static void Planted()
    {
        Debug.Log("planted");
    }

    private static void Radio()
    {
        Debug.Log("Radio");
    }

    private static void ComEngi()
    {
        Debug.Log("Combustion engine");
    }

    private void Start()
    {
        for (int i = 0; i < upgradeList.Length; i++)
        {
            valid.Add(upgradeList[i]);
        }
        if (upgrades.Count > 0)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].setup();
            }
        }

        loadData();
    }

    public List<Upgrade> getValid()
    {
        return valid;
    }

    public List<Upgrade> getDeck()
    {
        return deck;
    }

    public List<Upgrade> getUpgrades()
    {
        return upgrades;
    }

    private void loadData()
    {
        FindObjectOfType<UpgradeSaver>().loadData(gameObject.tag);
    }

}

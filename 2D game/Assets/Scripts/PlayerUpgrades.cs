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
        planted
    }

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
        })
    };

    //holds the equipped upgrades
    private List<Upgrade> upgrades = new List<Upgrade>();

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

    private void Start()
    {
        if (upgrades.Count > 0)
        {
            for (int i = 0; i < upgrades.Count; i++)
            {
                upgrades[i].setup();
            }
        }
    }

}

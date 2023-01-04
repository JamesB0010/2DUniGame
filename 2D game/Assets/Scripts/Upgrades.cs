using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrade
{
    private string name;
    private string description;
    public delegate void init();
    public init setup;

    public Upgrade(string name, string description, init setupFunction)
    {
        this.name = name;
        this.description = description;
        this.setup = setupFunction;
    }

    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

}


public class Upgrades: MonoBehaviour
{
    static public void InsElec()
    {
        Debug.Log("Upgrade Insulated Electronics");
    }

    static private void SeaCirc()
    {
        Debug.Log("Upgrade Sealed Cirrcuits");
    }

    static private void Planted()
    {
        Debug.Log("Upgrade Planted");
        Debug.Log("change");
    }
}

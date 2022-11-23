using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//the obsticle class will be a parent class of all obsticle types, this class will hold data/functionality which is present in all obstacles
public class Obsticle : MonoBehaviour
{
    #region fields
    //all obstacles store a reference to both players so they are able to effect the players
    //protected means entities outside of the class cannot access these fields but classes that inherit from this class are able to access the fields/methods
    protected Player player1;
    protected Player2 player2;

    #endregion

    #region methods

    protected void Start()
    {
        //set a reference to both players
        player1 = FindObjectOfType<Player>();
        player2 = FindObjectOfType<Player2>();
    }
    #endregion
}

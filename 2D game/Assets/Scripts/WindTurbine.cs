using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbine : Obsticle
{
    #region fields
    //this is a velocity used to push the player away and is also used to check if the player is looking in the opposite direction to the push direction
    private Vector3 push;
    #endregion

    #region private methods

    //the base class start method gets a reference to both players
    //the wind turbine start method initialises the push variable
    void Start()
    {
        base.Start();
        push = gameObject.transform.right * -0.1f;
    }

    //when a player collides with the turbine check if the player should be blown off course,
    //if they should be then blow the player off course
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != player1.gameObject && collision.gameObject != player2.gameObject)
        {
            //collision is not a player 
            return;
        }

        if (Vector3.Angle(push.normalized, collision.gameObject.transform.up.normalized) > 130)
        {
            //dont blow the player
            return;
        }
        if (collision.gameObject == player1.gameObject)
        {
            //blow the player off course
            player1.Push(push);

        }
        else if (collision.gameObject == player2.gameObject)
        {
            //blow the player off course
            player2.Push(push);
            
        }
    }

    #endregion

}

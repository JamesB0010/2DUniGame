using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastZone : Obsticle
{
    #region setup
    new void Start()
    {
        base.Start();
    }

    #endregion

    #region private methods

    //if the player is accelerating then reset their acceleration and decrement their boost meter
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player1.gameObject)
        {
            //player 1
            if (player1.Revs > 1)
            {
                player1.Revs = 1;
                player1.setBoost(player1.Boost - Time.deltaTime * 20);
            }
        }
        else if (collision.gameObject == player2.gameObject)
        {
            //player 2
            if (player2.Revs > 1)
            {
                player2.Revs = 1;
                player2.setBoost(player2.Boost - Time.deltaTime * 20);
            }
        }
    }
    #endregion
}

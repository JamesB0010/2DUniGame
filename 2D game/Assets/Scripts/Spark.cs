using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spark : EmpDrone
{

    //the spark will have a very similar behaviour to the emp drone, however it will have a different payload

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //once a target has been aquired the drone will move towards it at a constant speed
    //if the target gets far enough away from the drone it will no longer follow the target
    //this method has been overidden in this class so the sprite renderers sprite is not changed
    private new void trackTarget()
    {
        //if the game is over stop chasing the players
        if (player1.GameOn == false)
        {
            target = null;
            return;
        }
        gameObject.transform.position = gameObject.transform.position + ((target.transform.position - gameObject.transform.position).normalized * Time.deltaTime * speed);


        if ((target.transform.position - gameObject.transform.position).magnitude >= 30)
        {
            target = null;
            collisionCount = 0;
        }
    }

    IEnumerator animateBoostDepleate(Player player)
    {
       while(player.Boost > 0)
        {
            player1.setBoost(player1.Boost - Time.deltaTime * 150);
            yield return null;
        }
    }

    IEnumerator animateBoostDepleate(Player2 player)
    {
        while (player.Boost > 0)
        {
            player2.setBoost(player2.Boost - Time.deltaTime * 150);
            yield return null;
        }
    }

    private new void deliverPayload()
    {
        //check if this is player 1 or player2
        if (target == player1.gameObject)
        {
            //effect player 1
            StartCoroutine(animateBoostDepleate(player1));
            player1.gameObject.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            //effect player 2
            StartCoroutine(animateBoostDepleate(player2));
            player2.gameObject.GetComponent<ParticleSystem>().Play();
        }
        FindObjectOfType<AudioManager>().Play("ElectricZap");
    }


    //the update method controlls what behavious the spark exibits and at what time
    //first the spark must lock onto a player
    //then it must track towards the player
    void Update()
    {
        //1st step find lock on, if a target has not been aquired then look for one and then exit the function
        if (target == null)
        {
            findTarget();
            return;
        }

        //2nd step track target this is only run if a target has been found
        trackTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //prevent the emp from detonating when it shouldnt
        if (collision.gameObject != target)
        {
            return;
        }
        //once the drone has collided with the player for the second time explode
        if (collisionCount == 1)
        {
            deliverPayload();
            GetComponent<SpriteRenderer>().enabled = false;
        }
        collisionCount++;
    }
}


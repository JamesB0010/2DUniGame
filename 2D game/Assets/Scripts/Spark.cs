using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The spark has a similar but different function to the emp done
//the difference between the spark and the emp drome is the payload is different.
//The emp drone stops movement while the spark resets the boost meter.
public class Spark : EmpDrone
{
    #region Private methods

    //call the parent class' start method thereby initialising the player1 and player2 variables
    new void Start()
    {
        //subscribing to the GameOver event
        FindObjectOfType<GameManager>().GameOver += onGameOver;
        base.Start();
    }

    //event handler for when a new game commences
    public new void onGameOver()
    {
        Destroy(gameObject);
    }

    //once a target has been aquired the spark will move towards it at a constant speed
    //if the target gets far enough away from the spark it will no longer follow the target
    //this method has been overidden in this class so the sprite is not changed
    private new void trackTarget()
    {
        //if the game is over stop chasing the players
        if (player1.GameOn == false)
        {
            target = null;
            return;
        }

        //otherwise move towards the targwt
        gameObject.transform.position = gameObject.transform.position + ((target.transform.position - gameObject.transform.position).normalized * Time.deltaTime * speed);

        //if the player has moved far enough away then stop chasing
        if ((target.transform.position - gameObject.transform.position).magnitude >= 30)
        {
            target = null;
            collisionCount = 0;
        }
    }

    //this allows the boost meter tp drop down to zero gradually instead of being instantly set to zero, the boost meter visually dropping looks more appealing that it instantly dropping to zero
    IEnumerator animateBoostDepleate(Player player)
    {
        //until the players boost is 0 reduce the boost
       while(player.Boost > 0)
        {
            player1.setBoost(player1.Boost - Time.deltaTime * 150);
            yield return null;
        }
    }

    //this is an example of polymorphism the same function performes differently depending on what type parameter(s) it recieves this is called static polymorphism
    IEnumerator animateBoostDepleate(Player2 player)
    {
        while (player.Boost > 0)
        {
            player2.setBoost(player2.Boost - Time.deltaTime * 150);
            yield return null;
        }
    }

    //deliver the payload (reset the players boost)
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
        //once the spark has collided with the player for the second time deliver the payload
        if (collisionCount == 1)
        {
            deliverPayload();
            GetComponent<SpriteRenderer>().enabled = false;
        }
        collisionCount++;
    }

#endregion
}


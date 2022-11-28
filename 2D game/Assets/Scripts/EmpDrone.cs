using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpDrone : Obsticle
{

    #region fields
    //the target is the player who is being locked onto it is of type gameObject as player1 and player2 are different classes however they both have a gameObject of type GameObject
    protected GameObject target;

    //how fast the drone moves
    protected int speed = 5;

    //the drone explodes on the second collision with a player as the 1st collision is almoast guarenteeded to happen
    protected int collisionCount = 0;

    //how long the emp drone should wait before resetting the player speed
    protected int waitTime = 2;

    //the different sprites used for the drone
    public Sprite angryDrone;
    public Sprite normalDrone;

    #endregion

#region private methods
    private new void Start()
    {
        //subscibe to the GameOver event
        FindObjectOfType<GameManager>().GameOver += onGameOver;
        base.Start();
    }

    //when the game Over event fires handle it using this method
    private void onGameOver()
    {
        Destroy(gameObject);
    }

    //this function checks the positions of both players relative to the drone
    //once one of the players gets too close the drone will lock on to that player setting it as the target 
    //the drone should not be able to lock on if the race hasnt started yet
    protected void findTarget()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject playerObject;
            if (i == 0)
            {
                playerObject = player1.gameObject;
            }
            else
            {
                playerObject = player2.gameObject;
            }

            if (((playerObject.transform.position - gameObject.transform.position).magnitude < 13) && player1.GameOn == true)
            {
                target = playerObject;
                GetComponent<SpriteRenderer>().sprite = angryDrone;
            }

        }
    }

    //once a target has been aquired the drone will move towards it at a constant speed
    //if the target gets far enough away from the drone it will no longer follow the target
    protected void trackTarget()
    {
        //if the game is over stop chasing the players
        if (player1.GameOn == false)
        {
            GetComponent<SpriteRenderer>().sprite = normalDrone;
            target = null;
            return;
        }
        gameObject.transform.position = gameObject.transform.position + ((target.transform.position - gameObject.transform.position).normalized * Time.deltaTime * speed);


        if((target.transform.position - gameObject.transform.position).magnitude >= 30)
        {
            target = null;
            GetComponent<SpriteRenderer>().sprite = normalDrone;
            collisionCount = 0;
        }
    }


    //the update method controlls what behavious the drone exibits and at what time
    //first the drone must lock onto a player
    //then it must track towards the player
        private void Update()
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

    //the deliver payload function is triggered after the second collision with the player. It zeros the player speed and stops them, after a certain number of seconds the SustainPayload function resets the players speed
    protected void deliverPayload()
    {
        //check if this is player 1 or player2
        GetComponent<ParticleSystem>().Play();
        if (target == player1.gameObject)
        {
            //effect player 1
            player1.Speed = 0;
            player1.TurnSpeed = 0;
            player1.Stop();
            player1.gameObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(SustainPayload(player1));
        }
        else
        {
            //effect player 2
            player2.Speed = 0;
            player2.TurnSpeed = 0;
            player2.Stop();
            player2.gameObject.GetComponent<ParticleSystem>().Play();
            StartCoroutine(SustainPayload(player2));
        }
        FindObjectOfType<AudioManager>().Play("ElectricZap");
    }

    //the SustainPayload functions wait a certain number of seconds and then reset the players speed. once the players speed has been reset the drone can delete its self
    IEnumerator SustainPayload(Player player1)
    {
        yield return new WaitForSeconds(waitTime);
        player1.Speed = 1;
        player1.TurnSpeed = 2;
        Destroy(gameObject);
    }

    //this shows polymorphism as the function behaves differently depending on what type the parameter is. 
    IEnumerator SustainPayload(Player2 player2)
    {
        yield return new WaitForSeconds(waitTime);
        player2.Speed = 1;
        player2.TurnSpeed = 2;
        Destroy(gameObject);
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
    #endregion
}

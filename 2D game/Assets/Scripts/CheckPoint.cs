using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is the class for the checkpoints, checkpoints are positioned around the map. 
//checkpoints hold references to their neighbours thereby creating a graph data structure 
public class CheckPoint : MonoBehaviour
{
    #region fields
    
    //the neigbours of this checkpoint (the next checkpoint to traverse)
    public CheckPoint[] next;

    //each checkpoint stores the visual road routes (B-spline sprites) to all its neighbours allowing the player to easily find the next checkpoint
    //the roadRoutes array must be stored in the same order as the next array for example if this checkpoint was x and the 1st checkpoint in the next array was y
    //then the first item in the roadroutes array would be a connection between x and y
    public GameObject[] roadRoutes;

    //to prevent duplication when a checkpoint is added to the raceroute in the GameManager it must never be used again. 
    //to achecive this the valid list is the same as the next list to start with. as checkpoints are added to the race all references of them are removed from each checkpoints valid list
    public List<CheckPoint> valid = new List<CheckPoint> ();

    //the og valid list allows the valid list to be reset when setting up another race 
    public List<CheckPoint> ogValid = new List<CheckPoint>();


    #endregion

    #region private methods

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //find if it was player 1 or player 2 colliding with the checkpoint
        if (collision.CompareTag("Player"))
        {
            //tell the Game Manager player 1 has collided with the checkpoint
            //the collision will be sent to the game menager which will be able to check if it is the correct checkpoint if it is not the correct checkpoint the collision will be ignored
            //the collision will be invalid if the checkpoint is not enabled or the player has skipped a checkpoint
            FindObjectOfType<GameManager>().CheckpointReached(gameObject, collision.gameObject.GetComponent<Player>().hud, collision.gameObject);
        }
        if (collision.CompareTag("Player2"))
        {
            //tell the game manager player 2 has collided with the checkpoint
            FindObjectOfType<GameManager>().CheckpointReached(gameObject, collision.gameObject.GetComponent<Player2>().hud, collision.gameObject);
        }
    }

    #endregion

    #region public methods

    //remves a checkpoint reference from the valid list, as mentioned above in the fields region this prevents duplication
    public void Forget(CheckPoint checkPoint)
    {
        valid.Remove(checkPoint);
    }

    //make a path from checkpoint to checkpoint visable so the player is able to follow it to the next checkpoint
    public void activatePath(int index)
    {

        roadRoutes[index].GetComponent<Renderer>().enabled = true;
    }



    //disable all of the routes to the neighbouring checkpoints
    //this is used when generating a new race and can be thought of as wiping a chalk board clean so you can draw on it again without the previous drawing overlapping
    public void hideRoadRoutes()
    {
        for (int i = 0; i < roadRoutes.Length; i++)
        {
            roadRoutes[i].GetComponent<Renderer>().enabled = false;
        }
    }


    //returns the index of a checkpoint in the next array
    public int getIndexFromCheckpoint(CheckPoint point)
    {
        //iterate through each of the next elements
        for (int i = 0; i < next.Length; i++)
        {
            if (next[i] == point)
            {

                //return the index if it matches the checkpoint we are searching for
                return i;
            }
        }
        //if the checkpoint cant be found (this shouldnt happen) return -1
        return -1;
    }

    //reset the valid list and refit it using the og valid list
    public void Remember()
    {
        valid = new List<CheckPoint>();
        for (int i = 0; i < ogValid.Count; i++)
        {
            valid.Add(ogValid[i]);
        }
    }
    #endregion  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMeter : MonoBehaviour
{
    #region fields
    //this is used to make the bar move to show how much boost the player has
    private float boost;

    //the scrollBar which will move as the player gets more boost 
    public GameObject scrollBar;

    //lower bound upperbound ensure that the bar maps correctly between 0 - 100 and being all the way to the left and all the way to the right
    private float lowerBound = 41.6f;
    private float upperBound = 4.6f;

    //accounts for the screen distortion when boosting 
    private float lowerBoundBoost = 60f;

    //used to find if the player is boosting
    private float maxSpeed;

    //the player this UI belongs to
    public GameObject player;
    public string playerName;

    #endregion
 
    void Update()
    {
        //depending on what player this UI element is assigned to read a different variable
        if (playerName == "Player")
        {
            boost = player.GetComponent<Player>().Boost;
            maxSpeed = player.GetComponent<Player>().MaxSpeed;
        }
        else
        {
            boost = player.GetComponent<Player2>().Boost;
            maxSpeed = player.GetComponent<Player2>().MaxSpeed;
        }

        //the X position of the scrollBar
        float position;


        //if the player is boosting 
        if (maxSpeed > 9)
        {
            //position the scroll bar between being fully invisable and being fully visable proportionally to the players boost 
            position = ExtraFunctions.Map(boost, 0, 100, (gameObject.transform.position.x) - lowerBoundBoost, gameObject.transform.position.x - upperBound);
        }
        else
        {
            //position the scroll bar between being fully invisable and being fully visable proportionally to the players boost 
            position = ExtraFunctions.Map(boost, 0, 100, gameObject.transform.position.x - lowerBound, gameObject.transform.position.x - upperBound);
        }

        //update the scrollbars position
        scrollBar.transform.position = (new Vector3(position, scrollBar.transform.position.y, scrollBar.transform.position.z));
    }
}

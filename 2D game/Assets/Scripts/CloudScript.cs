using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{

    #region fields

    //how fast the fload moves from right to left
    //when the cload is spawned or is reset to the right of the map the speed is randomised
    private float speed = -0.3f;

    //ensures the random speed isnt too fast 
    private float maxSpeed = -5;

    #endregion

    #region methods
    void Start()
    {
        speed = Random.value * maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //move right 
        transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);

        //if we have reached the left of the map reset position and speed 
        //the x position is fixed but the y position is random between the top and bottom of the map
        if (transform.position.x <= -86)
        {
            transform.Translate(200, Random.value * 40 - 20,0);
            speed = Random.value * maxSpeed;
        }
    }

    #endregion
}

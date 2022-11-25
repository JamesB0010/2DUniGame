using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Player2 : MonoBehaviour
{
    #region fields
    //the hud for this player
    public Hud hud;

    //passed as a parameter to input.getAxis() when getting player input
    private string Horizontal = "Horizontal2";
    private string Vertical = "Vertical2";

    //stores if the race is active 
    public bool GameOn = false;

    public Rigidbody2D rigidBody;

    //the virtual camera tracking the player
    public CinemachineVirtualCamera vCam;

    //the game object on the hud that shows the character in the bottom right of the screen
    public GameObject coPilotUI;

    //https://forum.unity.com/threads/solved-change-ui-source-image.367215/ credit for changing ui images
    //the different images used by the copilotUI sprite renderer
    public Sprite unhappySprite;
    public Sprite happySprite;
    public Sprite superHappySprite;

    //when the player boosts both values go up over time to make the camera shake more violently and more frequently the longer the player boosts for
    private float camShakeMag = 0;
    private float camShakeFreq = 0;

    //Forwards and sideways input
    private float inputVertical;
    private float inputHorizontal;

    //Adding code to animate the player brackeys tutorial https://www.youtube.com/watch?v=hkaysu1Z-N8
    // the animator which controls the animations of the player
    public Animator animator;

    //this ensures the accelerate sound is only playing once and prevents the sound from being played every frame
    private bool replayAccelerateSound = true;

    //prevents the boost sound from being played multiple times and enables code to be executed for the frame the player starts boosting and no other frames
    private bool boostSoundReady = true;

    //this means the player must wait for their boost meter to fill up to a certain amount before they are able to use the boost 
    private bool minBoostMet = false;

    //the post process volume is global and adds the post process effects stored in a post process profile to the screen, it is used when the player activates their boost to swap the normal post process profile with the post profile used for boosting, once the boost has ended the post process profile is set back to neutral
    public PostProcessVolume volume;

    public PostProcessProfile profileNormal;

    public PostProcessProfile profileBoost;


    //flag variable used to exit out of punchboost function
    private bool endBoost = false;

    //allows the camera to smoothly zoom in and out when the player boosts 
    private float zoomOutTimer;
    private float zoominTimer;

    //this means the camera zooms out from its current position when the boost is activated instead of snapping to a pre set value
    private float zoomInFrom;
    private float zoomOutFrom;

    //friction represents the external forces that move the car sideways and cause it to rotate 
    private float friction = 0;

    //acceleration is reset every update and accumulates all of the forces that the car will experience, the acceleration is then added to the velocity which does not reset every frame. this is what allows the smooth changes in direction
    private Vector3 acceleration;

    //velocity is not reset every update and represents the direction and speed the car is travelling at 
    private Vector3 velocity = new Vector3(0, 0, 0);

    //when the player stops accelerating a turbo blow off valve sound will play, to stop the sound from becoming repetitive and annoying a minimum time must have passed since the last time the sound has been played, this field stores
    private float timeSinceBlowOff = 0f;

    //this stops the player from attempting a boost as soon as the boost has finished, this prevents the no boost sound effect from playing as soon as the boost ends 
    //capped at 5 to save memory, additionally it is not needed after 5 seconds
    private float timeSinceBoost;

    //this allows the boost ready sound effect to play once when the boost is ready
    private bool playBoostReady = true;

    //holds a reference to the boost meter which updates to graphically show the players boost amount 
    public BoostMeter boostMeter;

    //fields that have a propertiy associated with them
    //------------------------------------------------------------------------------------------------------------------------------------------------------------

    //driftGauge stores how much boost the player has accumulated once this value reaches 100 the player is able to use their boost
    private float driftGauge = 0;

    //the max speed of the player is by default 9. this is increased when the player activates their boost 
    //the associated property is read only this means nothing else can directly change the players max speed this makes the program more secure
    private float maxSpeed = 9;

    //revs will allow the car to accelerate over time and not move at a constant speed. Additionally revs controls how much influence the acceleration has on the velocity of the player
    //this means that the longer the player accelerates for the more percieved grip they have
    private float revs = 1;

    //roll speed has a similar function to revs
    //revs controll how fast the car speeds up and roll speed controlls how fast the car slows down
    private float rollSpeed;

    //this is declared higher up but has its own property associated with it 
    //inputVertical

    //multiplied by the revs 
    private float speed = 1;
    private float turnSpeed = 2f;

    #endregion

    #region properties

    // validation is used to ensure that the dirft gauge does not go above 100
    public float Boost
    {
        get { return driftGauge; }
        set
        {
            driftGauge = driftGauge < 100 ? value : 100;
            if (value > 100)
            {
                //if the drift gauge meets 100 then set the min boost met variable to true and play the boost ready sound effect if it needs to be played
                minBoostMet = true;
                if (playBoostReady == true)
                {
                    FindObjectOfType<AudioManager>().Play("BoostReady");

                    //after playing the sound effect set playBoostReady to false so that the sound effect doesnt play multiple times
                    playBoostReady = false;
                }
            }
        }
    }

    //read only returns the max speed of the player, this can be used to check if the player is boosting as when the player is boosting the max speed is set to a value above 9
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    //returns the value of revs
    //sets revs after validation
    public float Revs
    {
        get { return revs; }
        set
        {
            //validation checking the revs dont go over the max speed
            if (value > maxSpeed)
            {
                revs = maxSpeed;
            }
            //validation ensures the revs dont drop below 1
            else if (value < 1)
            {
                revs = 1;
                //if the revs fall to 1 enable the replay accelerate sound variable. allowing the BigRevs sound effect to be played
                //stop the big revs sound effect and instead play the idle sound as the player is not accelerating
                replayAccelerateSound = true;
                if (!FindObjectOfType<AudioManager>().isPlaying("BigRevs"))
                {
                    FindObjectOfType<AudioManager>().Stop("BigRevs");
                }
            }
            //if validation isnt needed to change value then set revs to equal value
            else
            {
                revs = value;
            }

            //set the revs in the animator to equal the revs in the code, inspired by brackys tutorial referenced above
            //this allows the animator to play an animation if the revs meet certain conditions
            animator.SetFloat("Revs", revs);

            //if the player is accelerating and the replay accelerate sound variable is set to true meaning the sound effect can be played then
            //stop playing the idle sound effect and start playign the accelerate sound effect
            if (revs > 1 && replayAccelerateSound)
            {
                //idleSound.Stop();
                if (!FindObjectOfType<AudioManager>().isPlaying("BigRevs"))
                {
                    FindObjectOfType<AudioManager>().Play("BigRevs");
                }
                //set the replayAccelerateSound variable to false so the accelerate sound doesnt play multiple times
                replayAccelerateSound = false;
            }
        }
    }


    //returns the roll speed
    //sets the roll speed after some validation
    public float RollSpeed
    {
        get { return rollSpeed; }
        set
        {
            //if the value is greater than the max speed set the roll speed to equal the max speed (validation)
            if (value > maxSpeed)
            {
                rollSpeed = maxSpeed;
            }
            //if the speed goes below 0 set the roll speed to equal 0 (validation)
            else if (value < 0)
            {
                rollSpeed = 0;
            }
            //if both validation checks pass then set the roll speed to equal the value
            else
            {
                rollSpeed = value;
            }
        }
    }

    //this returns the inputVertical field read only
    //this allows the player script to check if the accelerate sound should be stopped (if both players are not accelerating)
    public float InputVertical
    {
        get
        {
            return inputVertical;
        }
    }

    //this is accessed by the emp drone and used to disable the player from moving forwards
    public float Speed
    {
        set
        {
            speed = value;
        }
    }

    //this is accessed by the emp drone and used to disable the player from moving forwards
    public float TurnSpeed
    {
        set
        {
            turnSpeed = value;
        }
    }

    #endregion


    #region setup
    void Start()
    {
        //set the player and playerName variables in the appropriate hud to reference the appropriate values for this player
        hud.player = gameObject;
        hud.playerName = gameObject.name;

        //do the same for the boost meter
        boostMeter.player = gameObject;
        boostMeter.playerName = gameObject.name;
    }

    #endregion

    #region private methods

    //the update method has multiple parts
    //1. Gather player input 
    //2. Immediate responses to player input
    //3. Calculate acceleration
    //4. Add the acceleration to the velocity
    //5. Transform the player 
    //6. check for drifting
    void Update()
    {

        //1. Gather player input
        inputVertical = Input.GetAxisRaw(Vertical);
        inputHorizontal = Input.GetAxis(Horizontal);

        Debug.Log(inputVertical);
        Debug.Log(inputHorizontal);

        if (!GameOn)
        {
            inputVertical = 0;
        }

        //2. Immediate response to player input
        //check if the boost needs activating
        PunchBoost();

        //if the player has just let go of the accelerator then play the blow off sound, if the player is pressing the accelerator increase the time since blowoff variable
        float rawVert = Input.GetAxisRaw(Vertical);

        if (rawVert == 0 && timeSinceBlowOff >= 0.5f)
        {
            FindObjectOfType<AudioManager>().Play("BlowOff");
            timeSinceBlowOff = 0;
        }
        else if (rawVert == 1)
        {
            timeSinceBlowOff += Time.deltaTime;
        }


        //3. Calculate acceleration
        //The longer the player keeps inputting forwards the faster the car goes if they let go the revs drop and the car slows down

        //if the player is boosting increase their revs by a larger amount 
        if (maxSpeed > 9)
        {
            Revs = inputVertical > 0 ? Revs + 0.00003f * Time.deltaTime * 10000 : Revs - 0.1f * Time.deltaTime * 10000;
        }
        else
        {
            //when the player accelerates normally increase the revs by a normal value if they stop accelerating decrease the revs
            Revs = inputVertical > 0 ? Revs + 0.00003f * Time.deltaTime * 1000 : Revs - 0.1f * Time.deltaTime * 10000;
        }

        //if the player is accelerating increase the roll speed else decrease the roll speed
        RollSpeed = inputVertical > 0 ? RollSpeed + 0.006f * Time.deltaTime * 1000 : RollSpeed - 0.001f * Time.deltaTime * 100;

        //get the forwards speed of the car ready to be multiplied by the forwards direction 
        float forwardsSpeed = inputVertical * Time.deltaTime * Revs * speed;

        //reset acceleration 
        acceleration = new Vector3();

        //add the forwards direction to the acceleration 
        //you can use transform.up, down left and right to find this https://answers.unity.com/questions/251619/rotation-to-direction-vector.html, https://answers.unity.com/questions/1317241/transformforward-in-2d.html

        acceleration = transform.up * forwardsSpeed * Time.deltaTime * 5000;


        //now we need to find the friction acting sideways to the car
        friction = inputHorizontal / 100;

        //create a sideways acceleration vector and then add it to the acceleration
        Vector3 sidewaysAcceleration = new Vector3(transform.right.x, transform.right.y, transform.right.z) * friction * Time.deltaTime * turnSpeed;

        acceleration += sidewaysAcceleration;

        //reduce the acceleration so that its influence over the velocity isnt too heavy
        acceleration /= 1000;

        //if the game has not started reset the acceleration so the player cant move before the game has started
        if (!GameOn)
        {
            acceleration = new Vector3();
        }

        //4. add the acceleration to the velocity
        velocity += acceleration;

        //if the velocity is greater than the roll speed * 0.001f then clamp the magnitude of the velocity to prevent the player from going too fast
        //the roll speed is reduced by timesing it by 0.001 so that the velocitys magnitude isnt too much otherwse the player could go far to quickly
        if (velocity.magnitude > 0.001f * RollSpeed)
        {
            velocity = Vector3.ClampMagnitude(velocity, 0.001f * RollSpeed);
        }

        //times the velocity by delta time to ensure an equal speed despite framerate
        velocity *= Time.deltaTime * 700;

        //5. Transform the player
        //rotation
        transform.Rotate(new Vector3(0, 0, friction * Time.deltaTime * turnSpeed * -5000), Space.World);
        //position (translation)
        transform.Translate(velocity, Space.World);

        //6. check for drifting
        Drift();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //credit for how to avoid collisions https://forum.unity.com/threads/ignore-collisions-by-tag-solved.60387/
        //if the players have collided with each other then ignore the collision and allow the players to move through each other
        if (collision.gameObject.CompareTag("Player")){
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        //this fixes a bug when a new race starts the crash sound plays when it shouldnt, now the crash sound can onlt play once the race is active
        if (GameOn)
        {
            FindObjectOfType<AudioManager>().Play("Crash");
        }

        //this ensures that the sprite does not change if the player crashes when boosting
        if (maxSpeed != 15)
        {
            coPilotUI.GetComponent<Image>().sprite = unhappySprite;
        }


        //emmit a ray from the position of the player following its velocity 
        RaycastHit2D hit = Physics2D.Raycast(transform.position, velocity);
        if (hit)
        {
            //set the velocity to the reflection of the raycast, this allows the car to bounce off whatever it colides with
            velocity = Vector2.Reflect(velocity.normalized, hit.normal).normalized * 0.004f;

            //reset speed and revs so player has to re build up their speed
            Revs = 1;
            RollSpeed = 1;
        }
    }

    //this function is used to evaluate if the player is travelling fast enough to drift it returns true or false
    private bool IsDriftSpeed()
    {
        float speed = RollSpeed;
        if (rollSpeed < maxSpeed - 2)
        {
            return false;
        }
        return true;
    }

    //This function will work out if the player is drifting
    //it find the angle between the direction the car is facing and its velocity. if the direction is pointing x degrees away from the velocity this is classed as a drift
    private void Drift()
    {
        //guard clause to ensure the Boost can only accumulate when the player is travelling at drift speed
        if (IsDriftSpeed() == false)
        {
            return;
        }

        //find the angle difference of the direction the car is pointing and the direction of the velocity
        float angle = Vector2.Angle(transform.up.normalized, velocity.normalized);

        if (angle > 30)
        {
            //player is drifting
            Boost += Time.deltaTime * 10;
            //unless the player is boosting set the copilot image to be the happy sprite
            if (maxSpeed != 15)
            {
                coPilotUI.GetComponent<Image>().sprite = happySprite;
            }
        }
    }

    //this function makes the player boost it is made up of multiple parts
    //1. check if the player is pressing the input key associated with boosing 
    //2. check if the player has tried to boost when they cannot boost
    //3. increment the time since the last boost 
    //4. check if the boost has just ended
    //4.1 if it should end zoom back into the player

    //5 runs once when the boost ends

    //6 runs once before the boost starts
    //7 runs every frame when boosting
    //8 runs on the first frame when boosting
    private void PunchBoost()
    {
        //runs every frame
        //1. Check if the player is pressing the input key associated with boosting 
        int input = (int)Input.GetAxisRaw("Boost2");

        //check if controller button 0 (the boost button for controllers) has been pressed
        input = input == 0 ? (int)Input.GetAxisRaw("Boost2Controller") : input;


        if (input == 0 || driftGauge <= 0 || minBoostMet == false)
        {
            //2. Check if the player has tried to boost when they cannot boost
            if (input == 1 && minBoostMet == false && timeSinceBoost > 1)
            {
                //if the boost not ready sound isnt already playing play the boost not ready sound
                if (!FindObjectOfType<AudioManager>().isPlaying("BoostNotReady"))
                    FindObjectOfType<AudioManager>().Play("BoostNotReady");
                Debug.Log("Boost not ready");
            }

            //3. increment the time since the last boost if the player has just boosted they will not be able to boost again until this variable passes a certain threshold
            timeSinceBoost = timeSinceBoost > 5 ? 5 : timeSinceBoost + Time.deltaTime;

            //4. check if the boost has just ended
            if (endBoost == false)
            {
                if (zoominTimer > 0)
                {

                    //if the boost has just ended then zoom back in to the player
                    vCam.m_Lens.OrthographicSize = ExtraFunctions.Animate(zoomInFrom, 12.5f, 1f, zoominTimer);
                    zoominTimer += Time.deltaTime;

                    //once the camera has fully zoomed back in set zoominTimer to equal 0 so this function isnt needlessley ran
                    if (vCam.m_Lens.OrthographicSize == 12.5)
                    {
                        zoominTimer = 0;
                    }
                }
                return;
            }


            //5 runs once when boost ends
            //reset the copilot image
            coPilotUI.GetComponent<Image>().sprite = happySprite;

            //set the play boost ready variable to true so the sound effect can play when the boost meter has refilled
            playBoostReady = true;

            //reset the time since boost so the player cant attempt to boost straight after their last boost this stops the cant boost sound effect from playing straight away when the player finishes boosting
            timeSinceBoost = 0;

            //turn off the camera shake
            vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
            camShakeMag = 0;
            camShakeFreq = 0;

            //reset the max speed and reset the post processing effects 
            maxSpeed = 9;
            volume.profile = profileNormal;

            //make the animator switch from the boost animation to a different animation
            animator.SetBool("Boosting", false);

            //play the beep and finish boost sounds
            ExtraFunctions.BatchPlay(new string[] { "Beep", "FinishBoost" });

            //zoom back into the player
            zoomInFrom = vCam.m_Lens.OrthographicSize;
            vCam.m_Lens.OrthographicSize = ExtraFunctions.Animate(zoomInFrom, 12.5f, 1f, zoominTimer);
            zoominTimer += Time.deltaTime;

            //reset the zoom out timer
            zoomOutTimer = 0;

            //set the boost sound ready to be true so when the next boost is ready the sound effect will happen
            boostSoundReady = true;

            //stop playing the sounds associated with boosting
            ExtraFunctions.BatchStop(new string[] { "StartBoost", "Boost" });

            //resume the ambient music
            FindObjectOfType<AudioManager>().PausePlay(FindObjectOfType<AudioManager>().ambientMusic[FindObjectOfType<AudioManager>().selection], "Play");

            //set end boost to false so the contents of this if statement are only run once when the boost finishes
            endBoost = false;

            //update the minBoostMet variable
            if (Boost < 100)
            {
                minBoostMet = false;
            }

            //if the player is holding forwards play the accelerate sound
            if (input != 0)
            {
                if (!FindObjectOfType<AudioManager>().isPlaying("BigRevs"))
                {
                    FindObjectOfType<AudioManager>().Play("BigRevs");
                }
            }
            //the boost has ended return from the function
            return;
        }

        //6 runs once before the boost starts
        if (boostSoundReady)
        {
            //set the zoom out from variable to be the current zoom amount so it doesnt snap
            zoomOutFrom = vCam.m_Lens.OrthographicSize;

            //this is to stop/reduce the flicker when starting zooming out
            vCam.m_Lens.OrthographicSize += 0.000001f;
        }

        //7 runs every frame when boosting
        //pause the ambient music
        FindObjectOfType<AudioManager>().PausePlay(FindObjectOfType<AudioManager>().ambientMusic[FindObjectOfType<AudioManager>().selection], "Pause");

        //set the input vertical to be 1 this ensures when boosting the player will always be going forwards at max speed/acceleration even if they are not pressing the accelerate button
        inputVertical = 1;

        //set the max speed to be higher than default
        maxSpeed = 15;

        //depleate the drift gauge as the player continues to boost
        //22 was a number which seemed to cause the boost to depleate at a rate i was happy with after play testing
        driftGauge -= Time.deltaTime * 22;

        //zoom out from the player
        vCam.m_Lens.OrthographicSize = ExtraFunctions.Animate(zoomOutFrom, 18f, 0.8f, zoomOutTimer);
        zoomOutTimer += Time.deltaTime;


        //cam shake credit to code monkey https://www.youtube.com/watch?v=ACf1I27I6Tk
        //make the camera shake and make the magnitude of the camera shake and the freaquencey increase as the player boosts
        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = camShakeMag;
        vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = camShakeFreq;
        camShakeMag += Time.deltaTime;
        camShakeFreq += Time.deltaTime;

        //8 runs on the first frame when boosting
        if (boostSoundReady)
        {
            //set the copilot sprite to be super happy
            coPilotUI.GetComponent<Image>().sprite = superHappySprite;

            //swap the default post processing for boost post processing
            volume.profile = profileBoost;

            //play the sounds associated with boosting
            ExtraFunctions.BatchPlay(new string[] { "Boost", "StartBoost" });

            //stop the big revs sound from playing
            FindObjectOfType<AudioManager>().Stop("BigRevs");

            //make the animator play the boosting animation
            animator.SetBool("Boosting", true);

            //set the boost sound ready to false so the contents of this if statement only execute once 
            boostSoundReady = false;

            //set the endBoost to true so when the boost ends the code for when the boost ends will be executed once
            endBoost = true;
        }
    }

    #endregion

    #region public methods
    //reset the velocity and acceleration to stop the car from making extra movements
    public void Stop()
    {
        velocity = new Vector3();
        acceleration = new Vector3();
    }

    //adds a velocity to the players velocity, used by the wind turbines to push the player
    public void Push(Vector3 pushForce)
    {
        velocity += pushForce;
    }
    #endregion

}

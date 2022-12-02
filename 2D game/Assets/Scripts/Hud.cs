using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour
{

    #region fields

    //the object of text displaying the time (timeText) and the component which stores and displays the text textMesh
    public GameObject timeText;
    private TextMeshProUGUI textMesh;

    //the object of the text displaying the checkpoint progress (checkpointText) and the component which stores and displays the text checkpointTextMesh
    public GameObject checkpointText;
    private TextMeshProUGUI checkpointTextMesh;

    //the rev needle and the speedometer needle. They will rotate depending on how fast the player is travelling (speed) and for how long they have been accelerating (revs)
    public GameObject needleSpeed;
    public GameObject revNeedle;

    //the object of the text which tells the player the game is over and who won (gameOverText) and the component which stores and displays the text gameOverTextMesh
    public GameObject gameOverText;
    private TextMeshProUGUI gameOverTextMesh;

    //the object of the text which counts down to the start of each race (countDownText) and the component whoich stores and displays the text countDownTextMesh
    public GameObject countDownText;
    private TextMeshProUGUI countDownTextMesh;

    //stores the sum of the times of all the last races
    //this means each race the time starts at 0
    private float lastRaceTimes;

    //counts up when the game has started this allows the countdown at the start of the game to disapear at the correct time
    private float timeSinceStart;

    //the player which this hud relates to (player 1 or player 2)
    public GameObject player;

    //the name of the player's game object 
    public string playerName;

    #endregion

    #region properties

    //gets and sets the lastRaceTimes field
    public float LastRaceTimes
    {
        get
        {
            return lastRaceTimes;
        }
        set
        {
            lastRaceTimes = value;
        }
    }

    //gets and sets the timeSinceStart field
    //when setting the field validation is used 
    public float TimeSinceStart
    {
        get
        {
            return timeSinceStart;
        }
        set
        {
            //limit the maximum value of this variable to be 6. the variable is not useful after the countdown at the start of the race (after the first 6 seconds of the race)
            //this also saves memory as the timeSinceStart variable doesnt get bigger the longer the player plays the game
            if (value > 6)
            {
                timeSinceStart = 6;
            }
            else
            {
                timeSinceStart = value;
            }
        }
    }

    #endregion

    #region setup
    void Start()
    {
        //set the variables to the appropriate components 
        textMesh = timeText.GetComponent<TextMeshProUGUI>();
        checkpointTextMesh = checkpointText.GetComponent<TextMeshProUGUI>();
        gameOverTextMesh = gameOverText.GetComponent<TextMeshProUGUI>();
        countDownTextMesh = countDownText.GetComponent<TextMeshProUGUI>();

        //by default the hud elements assosiated with the end of a race are set to be invisable 
        gameOverTextMesh.enabled = false;
        //credit for how to get a child object https://stackoverflow.com/questions/40752083/how-to-find-child-of-a-gameobject-or-the-script-attached-to-child-gameobject-via#:~:text=Finding%20child%20GameObject%20by%20index%3A&text=transform.,3%2C%20to%20the%20GetChild%20function.

        //start tracking time and updating the ui
        StartCoroutine("trackTime");
    }
    #endregion

    #region private methods
    void Update()
    {
        //animate the speedometer
        float rollSpeed = 0;

        //get the correct players roll speed and assign it to the rollSpeed variable
        if (playerName == "Player")
        {
            rollSpeed = player.GetComponent<Player>().RollSpeed;
        }
        else
        {
            rollSpeed = player.GetComponent<Player2>().RollSpeed;
        }

        //calculate how much the speedometers needle should rotate by mapping the current speed from the range of 0 - max speed to the range of min and max rotation
        float rotation = ExtraFunctions.Map(rollSpeed, 0, 15, 170f, -26.57f);
        //rotate the needle
        needleSpeed.transform.eulerAngles = new Vector3(0,0,rotation);


        //animate the rev meter
        float revs = 0;

        //get the correct players revs and assign it to the revs variable
        if (playerName == "Player")
        {
            revs = player.GetComponent<Player>().Revs;
        }
        else
        {
            revs = player.GetComponent<Player2>().Revs;
        }
        
        //calculate how much the rev meters needle should rotate by mapping the current revs from the range of 1 - 1.5 revs to the range of min and max rotation
        //this is not between 1 and max revs because the revs climb really slowly and the rev meter seems to be very unresponsive, by abstracting the actual value of the players revs from the player the game feels more responsive as the rev needle moves quicker and more like what the player would expect
        rotation = ExtraFunctions.Map(revs, 1f, 1.5f, 325f, 92f);

        //stops the needle from circling from max revs to min revs when boosting 
        if (rotation < 5)
        {
            rotation = 5;
        }

        //rotate the needle
        revNeedle.transform.eulerAngles = new Vector3(0,0,rotation);

        //stops the needle from rotating past the max
        if (revNeedle.transform.eulerAngles.z < 92f)
        {
            revNeedle.transform.eulerAngles = new Vector3(0, 0, 92f);
        }


        //inrement the time SinceStart
        TimeSinceStart += Time.deltaTime;

        //once the countdown has ended (after 6 seconds) disable the countdown element as it is no longer necessary
         if (TimeSinceStart >= 6)
        {
            countDownTextMesh.enabled = false;
        }

    }
    #endregion

    #region public methods

    //updates the text displaying the players progress through the race
    public void updateCheckpointText(int currentPoint, int totalPoints)
    {
        //if this function is called before the start function and the checkpointTextMesh has not yet been assigned a value then set the checkpointTextMesh's value now
        if (checkpointTextMesh == null)
        {
            checkpointTextMesh = checkpointText.GetComponent<TextMeshProUGUI>();
        }
        //update the text
        checkpointTextMesh.text = "Checkpoint " + currentPoint + "/" + totalPoints;
    }

    //when the race is over display the appropriate elemets
    //this function is called on both instances of the hud one will be passed 1 as a parameter and the other will be passed 2 as a parameter
    public void RaceOver(int position)
    {
        //if this is the hud of the winning player set the gameOverTextMesh to reflect this player won
        //else set the text to reflect that this player lost
        if (position == 1)
        {
            gameOverTextMesh.text = "Race Over! \n You Win!";
        }
        else
        {
            gameOverTextMesh.text = "Race Over! \n You Lose!";
        }

        //enable the appropriate visual elements
        gameOverTextMesh.enabled = true;
    }

    //sets the countdown textMesh to a number
    public void SetCountDownText(int number)
    {
        //make sure the text is visable
        countDownTextMesh.enabled = true;

        //instead of counting down to zero countdown to go
        if (number <= 0)
        {
            countDownTextMesh.text = "GO";
        }
        else
        {
            //if we shouldnt display "Go" display the number 
            countDownTextMesh.text = number.ToString();
        }
        
    }

    //when a race is started the hud elements associated with the end of the race are hidden
    public void disableReplayElements()
    {
        //if this function is called before the start function and the gameOverTextMesh has not yet been assigned a value then access it via the gameOverText object reference
        if (gameOverTextMesh == null)
        {
            gameOverText.GetComponent<TextMeshProUGUI>().enabled = false;
        }
        else
        {
            //disable the text
            gameOverTextMesh.enabled = false;
        }
    }

    //this function tracks the time however it waits 6 seconds before tracing the time as the countdown at the start of the game is 6 seconds long
    public IEnumerator trackTime()
    {
        yield return new WaitForSeconds(6);
        while (1 == 1)
        {
            //format the elapsed time since the start of the race to be 00:00
            // https://answers.unity.com/questions/45676/making-a-timer-0000-minutes-and-seconds.html
            float raceTime = Time.time - lastRaceTimes - 5;
            string minuets = (int)raceTime / 60 < 10 ? "0" + ((int)raceTime / 60).ToString() : ((int)raceTime).ToString();
            string seconds = (int)raceTime % 60 < 10 ? "0" + ((int)raceTime % 60).ToString() : ((int)raceTime % 60).ToString();
            textMesh.text = minuets + ":" + seconds;
            yield return null;
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region fields

    public delegate void onRaceOver();
    public event onRaceOver OnRaceOver;

    private static int checkpointPerRace = 10;

    //the raceRoute is an array of length checkpointperRace containing all of the checkpoints in a race. it is reset everytime a new race is generated
    public CheckPoint[] raceRoute = new CheckPoint[checkpointPerRace];

    //an array holding all of the checkpoints 
    public CheckPoint[] checkPoints;

    //each time a player reaches a valid checkpoint there progress is incremented
    //the progress is used to check if the checkpoint a player has just reached is the next one in the race therefore validating or invalidating it
    private int player1Progress = 0;
    private int player2Progress = 0;

    //used the set the checkpoint the player spawns on as green the first checkpoint acts as a start line
    private Color startColor = Color.green;

    //gameOn is false before the contdown timer says go and at the end of the race. it is used to prevent the player from moving when the race is not active
    private bool gameOn = false;

    //used to countdown and start the race, this variable is sent to the hud everyframe so the countdown on the Ui can countdown
    private float startCountDown = 6;

    //allows each race to start on 0 instead of the total time the player has been playing
    private float lastRaceTimes = 0;

    //a list of drone spawns so drones can be spawned onto the map when a new race is generated
    public List<GameObject> droneSpawns = new List<GameObject>();

    //a reference to the drone prefab
    public GameObject empDronePrefab;

    //the max inclusive value used when deciding what drones will spawn
    private int chance = 3;

    //a list of spaark spawns so sparks can be spawned when a new race is generated
    public List<GameObject> sparkSpawns = new List<GameObject>();

    //a reference to the spark prefab
    public GameObject sparkPrefab;


    #endregion

    #region setup
    void Start()
    {
        newRace();
    }
    #endregion

    #region private methods

    //this function returns a list of spawn points to spawn an emp drone at
    //it does this by looping over each position in the droneSpawns list and creating a random number between 1 and chance. if the number is chance the drone spawn is added to the list which is returned.
    private List<GameObject> generateSpawnPointsEmp()
    {
        List<GameObject> spawnPoints = new List<GameObject>();
        for (int i = 0; i < droneSpawns.Count; i++)
        {
            int randomNumber = Random.Range(1, chance);
            if (randomNumber == 1)
            {
                spawnPoints.Add(droneSpawns[i]);
            }
        }
        return spawnPoints;
    }

    //this function loops through each of the spawn points and instantiates a drone at each spawn point
    private void spawnDrones(List<GameObject> spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Instantiate(empDronePrefab, new Vector3(spawnPoints[i].transform.position.x, spawnPoints[i].transform.position.y, spawnPoints[i].transform.position.z), Quaternion.identity);
        }
    }

    //sparks will be spawed in the same as the drones
    private List<GameObject> generateSpawnpointsSpark()
    {
        List<GameObject> spawnPoints = new List<GameObject>();
        for (int i = 0; i < sparkSpawns.Count; i++)
        {
            int randomNumber = Random.Range(1, chance);
            if (randomNumber == 1)
            {
                spawnPoints.Add(sparkSpawns[i]);
            }
        }
        return spawnPoints;
    }

    private void spawnSparks(List<GameObject> spawnPoints)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Instantiate(sparkPrefab, new Vector3(spawnPoints[i].transform.position.x, spawnPoints[i].transform.position.y, spawnPoints[i].transform.position.z), Quaternion.identity);
        }
    }

    //this hides all of the visual connections between checkpoints
    void invisibleRoutes()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].hideRoadRoutes();
        }
    }

    //this hides all of the checkpoints
    void invisibleCheckPoints()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }
    }

    void Update()
    {

        //if the race has not started yet count down
        if (startCountDown > 0)
        {
            startCountDown -= Time.deltaTime;
            foreach(Hud hud in FindObjectsOfType<Hud>())
            {
                //update each players hud showning the countdown
                hud.SetCountDownText((int)startCountDown);
            }

            //once the start countdown reaches 0 start the race
            if (startCountDown <= 0)
            {
                //set GameManager.gameOn to be true and set the players GameOn properties to be true
                gameOn = true;
                FindObjectOfType<Player>().GameOn = gameOn;
                FindObjectOfType<Player2>().GameOn = gameOn;
            }
        }
    }

    //this tells all of the checkpoints to remove a checkpoint from their valid list thereby ensuring that there is no/reduced duplication
    //it effectivley disables the edge connection between two verticies (checkpoints) on the graph which makes up all the checkpoints
    private void CheckpointsForget(GameObject checkPointToForget)
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].GetComponent<CheckPoint>().Forget(checkPointToForget.GetComponent<CheckPoint>());
        }
    }

    //this does the opposite of the CheckpointsForget method
    //it tells all of the checkpoints to reset their valid list, this allows new races to be generated without the previous races effecting the probability of any of the checkpoints being included in the race route
    //it effectivley resets the graph which makes up all the checkpoints
    private void CheckPointsRemember()
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].GetComponent<CheckPoint>().Remember();
        }
    }

    //sets up a new race this is done by:
    //1. resetting variables needed for the race
    //2. reseting the visual elements needed for the race
    //3. randomly generating a new race
    //4. updating the necessary visual elements
    //5. setting the players location
    //6.spawning the EmpDrone obstacles
    private void newRace()
    {
        //1. resetting variables needed for the race
        lastRaceTimes = Time.time;
        startCountDown = 6;
        player1Progress = 0;
        player2Progress = 0;


        //2. reseting the visual elements needed for the race
        //first reset the necessary hud elements for both players huds
        foreach (Hud hud in FindObjectsOfType<Hud>())
        {
            hud.updateCheckpointText(player1Progress + 1, checkpointPerRace);
            hud.LastRaceTimes = lastRaceTimes;
            hud.TimeSinceStart = 0;
            hud.disableReplayElements();
        }
        //then reset the visual elements in the playspace (checkpoints and routes between checkpoints)
        invisibleCheckPoints();
        invisibleRoutes();


        //3. randomly generating a new race
        //3.1. reset the necessary variables
        //3.2 randomly decide the starting point for the race
        //3.3 traverse the graph randomly making checkpointPerRace - 1 traversals adding each checkpoint to the raceRoute array
        //3.3.1 note the graph is randomly traversed instead of just picking random checkpoints as it allows the race route to flow and not force the player to frequently turn back on themselves which doesnt work very well with the player controlls
        //3.4 tell all checkpoints to forget the checkpoint when it is added to the race route array


        //3.1 reset the necessary variables 
        //reset the checkpoints graph
        CheckPointsRemember();
        //reset the raceroute array
        raceRoute = new CheckPoint[checkpointPerRace];


        //3.2 randomly decide the starting point for the race
        int numberOfCheckPoints = FindObjectsOfType<CheckPoint>().Length - 1;
        raceRoute[0] = (checkPoints[Random.Range(0, numberOfCheckPoints)]);

        //make all checkpoints forget about the 1st checkpoint to avoid duplication
        CheckpointsForget(raceRoute[0].gameObject);


        //3.3 traverse the graph randomlly making checkpointPerRace - 1 traversals adding each checkpoint to the raceroute array
        //i is the last item in the array
        for (int i = 1; i < checkpointPerRace; i++)
        {
            //look at the node before i and get a random number between 0 and the number of connected checkpoints
            int randomNumber = Random.Range(0, raceRoute[i - 1].valid.Count);

            //if there is no valid checkpoints choose a random checkpoint from the next list (regardless of if has already been chosen)
            if (raceRoute[i - 1].valid.Count == 0)
            {
                raceRoute[i] = raceRoute[i - 1].next[Random.Range(0, raceRoute[i - 1].next.Length - 1)];
            }
            else
            {
                //make the most recent checkpoint (i) equal a random connected checkpoint from the previous checkpoint
                raceRoute[i] = raceRoute[i - 1].valid[randomNumber];
            }


            //3.4 tell all checkpoints to forget the checkpoint when it is added to the race route array
            CheckpointsForget(raceRoute[i].gameObject);
        }

        //4. updating the necessary visual elements
        //make the first 2 checkpoint connections visable
        raceRoute[0].activatePath(raceRoute[0].getIndexFromCheckpoint(raceRoute[1]));
        raceRoute[1].activatePath(raceRoute[1].getIndexFromCheckpoint(raceRoute[2]));

        //make the checkpoint after the start point cyan
        raceRoute[1].gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;

        //5. resetting the players location
        //set the players locations to be the starting checkpoint
        FindObjectOfType<Player>().gameObject.transform.position = raceRoute[0].gameObject.transform.position;
        FindObjectOfType<Player2>().gameObject.transform.position = raceRoute[0].gameObject.transform.position;

        //spawn the drones
        spawnDrones(generateSpawnPointsEmp());

        //spawn the sparks
        spawnSparks(generateSpawnpointsSpark());
    }

    #endregion

    #region public methods
    //The NewRace method is public meaning it can be called when the replay button is pressed 
    public void NewRace()
    {
        //reload the scene
        Application.LoadLevel("Main Game");
    }

    //this method is called by the checkpoints when a player collides with them, it is up to this method to validate and act on these collisions
    //1. check if it was player 1 or player 2 colliding with the checkpoint
    //2. check if the checkpoint collided with is the next checkpoint 
    //  2.1 update hud
    //  2.2 update necessary visual elements
    //  2.3 increment the players progress
    //3. check if the checkpoint collided with is the last checkpoint
    //  3.1. update necessary visual elements
    //  3.2. update necessary variables
    public void CheckpointReached(GameObject checkPoint, Hud hud, GameObject player)
    {
        //1. check if it was player 1 or player 2 colliding with the checkpoint
        if (player.name == "Player")
        {

            //2. check if the checkpoint collided with is the next checkpoint 
            if (checkPoint == raceRoute[player1Progress].gameObject)
            {

                //3. check if the checkpoint collided with is the last checkpoint
                if (player1Progress == checkpointPerRace - 1)
                {
                    //only end the race if the race is currently running, if the race has already ended dont run this code
                    if (gameOn)
                    {

                        //3.1. update necessary visual elements
                        hud.updateCheckpointText(player1Progress + 1, checkpointPerRace);
                        checkPoint.GetComponent<ParticleSystem>().startColor = Color.green;
                        checkPoint.GetComponent<ParticleSystem>().Play();
                        hud.RaceOver(1);
                        FindObjectOfType<Player2>().hud.RaceOver(2);

                        //3.2. update necessary variables
                        gameOn = false;
                        FindObjectOfType<Player>().GameOn = gameOn;
                        FindObjectOfType<Player2>().GameOn = gameOn;
                        OnRaceOver();
                        return;
                    }

                }

                //2.1 update hud
                //if the checkpoint collided with is the next checkpoint update the hud 
                hud.updateCheckpointText(player1Progress + 1, checkpointPerRace);


                //  2.2 update necessary visual elements                
                //if the checkpoint colided with is the first checkpoint then turn it green
                //else turn it cyan and play the particle system attached to the checkpoint 
                if (checkPoint == raceRoute[0].gameObject)
                {
                    checkPoint.GetComponent<SpriteRenderer>().color = startColor;
                }
                else
                {
                    checkPoint.GetComponent<SpriteRenderer>().color = Color.cyan;
                    checkPoint.GetComponent<ParticleSystem>().Play();
                }

                //enable the visuals for the next the path and the path after if there is one 
                //update the color of the next + 1 checkpoint
                raceRoute[player1Progress].activatePath(raceRoute[player1Progress].getIndexFromCheckpoint(raceRoute[player1Progress + 1]));
                if (player1Progress < checkpointPerRace - 2)
                {
                    raceRoute[player1Progress + 1].activatePath(raceRoute[player1Progress + 1].getIndexFromCheckpoint(raceRoute[player1Progress + 2]));
                    raceRoute[player1Progress + 2].gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                //  2.3 increment the players progress
                player1Progress++;
                //set the next checkpoints color to yellow
                raceRoute[player1Progress].GetComponent<SpriteRenderer>().color = Color.yellow;
            }

        }

        //the elseif statement is the same as the if statement however it is for player 2
        //1. check if it was player 1 or player 2 colliding with the checkpoint
        else if (player.name == "Player2")
        {

            //2. check if the checkpoint collided with is the next checkpoint 
            if (checkPoint == raceRoute[player2Progress].gameObject)
            {

                //3. check if the checkpoint collided with is the last checkpoint
                if (player2Progress == checkpointPerRace - 1)
                {
                    //only end the race if the race is currently running, if the race has already ended dont run this code
                    if (gameOn)
                    {

                        //3.1. update necessary visual elements
                        hud.updateCheckpointText(player2Progress + 1, checkpointPerRace);
                        checkPoint.GetComponent<ParticleSystem>().startColor = Color.green;
                        checkPoint.GetComponent<ParticleSystem>().Play();
                        hud.RaceOver(1);
                        FindObjectOfType<Player>().hud.RaceOver(2);

                        //3.2. update necessary variables
                        gameOn = false;
                        FindObjectOfType<Player>().GameOn = gameOn;
                        FindObjectOfType<Player2>().GameOn = gameOn;
                        return;
                    }

                }

                //2.1 update hud
                //if the checkpoint collided with is the next checkpoint update the hud 
                hud.updateCheckpointText(player2Progress + 1, checkpointPerRace);

                //  2.2 update necessary visual elements                
                //if the checkpoint colided with is the first checkpoint then turn it green
                //else turn it cyan and play the particle system attached to the checkpoint 
                if (checkPoint == raceRoute[0].gameObject)
                { 
                    checkPoint.GetComponent<SpriteRenderer>().color = startColor;
                }
                else
                {
                    checkPoint.GetComponent<SpriteRenderer>().color = Color.cyan;
                    checkPoint.GetComponent<ParticleSystem>().Play();
                }


                //enable the visuals for the next the path and the path after if there is one 
                //update the color of the next + 1 checkpoint
                raceRoute[player2Progress].activatePath(raceRoute[player2Progress].getIndexFromCheckpoint(raceRoute[player2Progress + 1]));
                if (player2Progress < checkpointPerRace - 2)
                {
                    raceRoute[player2Progress + 1].activatePath(raceRoute[player2Progress + 1].getIndexFromCheckpoint(raceRoute[player2Progress + 2]));
                    raceRoute[player2Progress + 2].gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
                }

                //  2.3 increment the players progress
                player2Progress++;

                //set the next checkpoints color to yellow
                raceRoute[player2Progress].GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }

    }

    #endregion 
}

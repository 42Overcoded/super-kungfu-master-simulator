using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int currentRound = 0;
    int difficulty_setting = 0; //0 = easy, 1 = medium, 2 = hard
    public float current_difficulty = 1; //the difficulty will depend on the current round and the difficulty setting. It should be used to determine the number of enemies and their strength
    float roundTime = 60f; //Duration of each round in seconds
    float gameTime = 0f; //Variable to keep track of the time if the current round
    float transitionTimer = 0f; //Timer to keep track of the time between rounds
    public int score = 0;
    int gameState = 0; //0 = round starting, 1 = round in progress, 2 = round ended, 3 = game over
    GameObject player;
    public int numberOfEnemies = 0; //placeholder, number of enemies remaining in the round, this should be updated to function properly with the enemy spawner

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player"); //Update this to the correct class once implemented

        current_difficulty += difficulty_setting * 2f; //Initialize the difficulty based on the difficulty setting
    }

    // Update is called once per frame
    void Update()
    {
        //Updates the current round time
        gameTime += Time.deltaTime;

        if (false) { //this should check the life of the player
            gameState = 3; //Change to game over state if player is dead
            Debug.Log("Player is dead!");
        }
        roundUpdate();
    }

    void roundUpdate()
    {
        switch (gameState) { //simple state machine to handle game states
            case 0: //Round start
                transitionTimer += Time.deltaTime;

                if (transitionTimer >= 3f) { //Wait for 3 seconds before starting the round
                    startRound();
                    gameState = 1; //Change to round in progress
                    Debug.Log("Round started! Current round: " + currentRound);
                }
                break;

            case 1: //Round in progress
                    //a round either ends when the time runs out or when all enemies are defeated
                    //bonus points will be awarded for defeating all enemies before the time runs out
                if (gameTime >= roundTime || numberOfEnemies <= 0) {
                    //If the round time is over or there are no enemies left, end the round
                    if (numberOfEnemies <= 0) {
                        AddScore((int)(roundTime)); //Bonus points for defeating all enemies
                        Debug.Log("All enemies defeated! Bonus points awarded.");
                    } else {
                        Debug.Log("Time's up! No bonus points awarded.");
                    }
                    gameState = 2; //Change to round ended
                    transitionTimer = 5f; //Set transition time before next round starts
                    Debug.Log("Round ended! Score: " + score);
                }
                break;

            case 2: //Round ended
                transitionTimer += Time.deltaTime;
                if (transitionTimer >= 3f) {
                    startRound(); //Start a new round
                    gameState = 0; //Change back to round starting
                }
                break;
            case 3: //Game over
                Debug.Log("Game Over! Final Score: " + score);
                break;
        }
    }

    //Starts a new round
    private void startRound()
    {
        currentRound++;
        gameTime = 0f;
        transitionTimer = 0f;
        updateDifficulty();

        Debug.Log("Starting round: " + currentRound);
        Debug.Log("Spawn enemies");
    }

    void updateDifficulty()
    {
        current_difficulty += 1 + difficulty_setting * 0.5f; //Example formula to calculate difficulty based on round and difficulty setting
    }

    //Used whenever score needs to be updated
    public void AddScore(int points)
    {
        score += points;
    }
    
    public void SetDifficulty(int difficulty)
    {
        difficulty_setting = difficulty;
        Debug.Log("Difficulty set to: " + difficulty_setting);
    }
}

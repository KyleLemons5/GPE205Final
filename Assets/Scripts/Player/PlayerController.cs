using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;
    public KeyCode shootKey;
    public KeyCode reloadKey;
    public KeyCode jumpKey;
    public int maxLives;
    public int lives;

    // Start is called before the first frame update
    public override void Start()
    {
        if(GameManager.instance != null)
        {
            GameManager.instance.players.Add(this);
            Debug.Log("controller added");
        }
        lives = maxLives;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        if(pawn != null)
        {
            ProcessInputs();

            base.Update();
        }
        else {
            if(lives > 0){
                GameManager.instance.SpawnPlayerPawn();
            }
            else{
                GameManager.instance.ActivateGameOver();
            }
        }
    }
    
    public override void ProcessInputs()
    {
        base.ProcessInputs();

        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
            pawn.MakeNoise();
        }
        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
            pawn.MakeNoise();
        }
        if(Input.GetKey(moveLeftKey))
        {
            pawn.MoveLeft();
        }
        if(Input.GetKey(moveRightKey))
        {
            pawn.MoveRight();
        }
        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();
            pawn.MakeNoise();
        }
        if(Input.GetKeyDown(reloadKey))
        {
            pawn.Reload();
        }
        if(Input.GetKeyDown(jumpKey))
        {
            pawn.Jump();
        }

        if(!Input.GetKey(moveForwardKey) && !Input.GetKey(moveBackwardKey) && !Input.GetKeyDown(shootKey))
            pawn.StopNoise();
    }

    public void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.players.Remove(this);
        }
    }

    // Adds an amount to score
    public override void AddToScore(float amount)
    {
        score += amount;
        //if(score > GameManager.instance.highscore){
            //GameManager.instance.highscore = score;
        //}
        Debug.Log(score);
    }
}

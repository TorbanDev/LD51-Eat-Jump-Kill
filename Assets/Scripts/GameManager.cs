using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip deathClip;
    public AudioClip spawnPUpClip;


    public UIManager uIManager;
    //Timer handling
    public float gameTimer = 10f;
    float gameTimerMax = 10;
    //Game handling
    int activeGameIndex = 2;
    int[] gameIndices = { 0, 1, 2 };
    public Minigame[] gameList;

    public bool StopCycling = false;

    //Rotation handling
    public GameObject rotator;
    public float rotationSpeed;
    public float rotationTime;
    bool rotating;
    int[] rotations = { 0, 120, 240 };
    int targetRotation;
    



    // Gameplay
    public int lives = 3;
    public enum GameState { PAUSED, PLAY };
    GameState gameState = GameState.PAUSED;

    // Powerups
    public int ShooterPowerUps = 0;
    public int SnakePowerUps = 0;
    public int JumpPowerUps = 0;

    public List<GameObject> UpgradeUiItems = new List<GameObject>();

    int foodRequiredForPUp = 3;

    public void ChangePowerUp(string p,int change)
    {
        switch (p) {
            case "SHOOTER_POWERUP":
                {
                    ShooterPowerUps += change;
                    AddShooterPowerUp();
                    break;
                }
            case "SNAKE_POWERUP":
                {
                    UpdateSnakePowerUps(change);
                    AddSnakePowerUp();
                    break;
                }
            case "JUMP_POWERUP":
                {
                    JumpPowerUps += change;
                    AddJumpPowerUp();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void GameOver(bool win)
    {
        gameState = GameState.PAUSED;
        PauseGame();
  
        uIManager.GameOver(win);
    }
    public void AddUpgrade(string type)
    {
        if (type == "HEALTH")
        {
            GainLife();
            return;
        }
        Debug.Log("ADDING UPGRADE: " + type);
        GameObject uiObj = GetObject(UpgradeUiItems);
        if (uiObj == null) return;
        Debug.Log("GOT A UI OBJ " + uiObj.name);
        UIUpgrade upgrade = uiObj.GetComponent<UIUpgrade>();
        upgrade.Initialize(type);
        uiObj.SetActive(true);
    }

    public GameObject GetObject(List<GameObject> objList)
    {
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].activeSelf == false)
            {
                return objList[i];
            }
        }
        return null;
    }


    private void UpdateSnakePowerUps(int change)
    {
        SnakePowerUps += change;
        AddSnakePowerUp();
        if (SnakePowerUps>= foodRequiredForPUp)
        {
            UpdateSnakePowerUps(-foodRequiredForPUp);
            ChangePowerUp("JUMP_POWERUP", 1);
        }
    }
    void AddJumpPowerUp()
    {
        uIManager.UpdateJumpPowerUpUI(JumpPowerUps);
    }
    void AddSnakePowerUp()
    {
        uIManager.UpdateSnakePowerUpUI(SnakePowerUps);
    }
    void AddShooterPowerUp()
    {
        uIManager.UpdateShooterPowerUpUI(ShooterPowerUps);
    }
    public void ChangeShooterPowerUp(int change) {
        ShooterPowerUps += change;
        AddShooterPowerUp();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("StartGame", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState==GameState.PLAY)
        {

            gameTimer -= Time.deltaTime;
            if (gameTimer<=0) {
                gameTimer = gameTimerMax;
                EveryTenSeconds();
            }

            if (rotating)
            {
                float rotationWindow = Math.Abs(targetRotation - rotator.transform.eulerAngles.z);
                if (rotationWindow <= 5)
                {
                    rotator.transform.localEulerAngles = new Vector3(0, 0, targetRotation);
                    rotating = false;
                    return;
                }

                rotator.transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));
            }
        }

    }
    void StartGame()
    {
        EveryTenSeconds();
    }
    private void EveryTenSeconds()
    {
        if(!StopCycling)
        {
            Debug.Log("10 seconds!");
            PauseGame();
            UpdateIndex();
            UpdateTargetRotation();
            StartCoroutine(rotate());
        }
    }

    private void UpdateTargetRotation()
    {
        targetRotation = rotations[activeGameIndex];
    }

    private void UpdateIndex()
    {
        if (activeGameIndex == gameIndices.Length - 1)
        {
            activeGameIndex = 0;
        }
        else activeGameIndex++;
    }

    IEnumerator rotate()
    {
        rotating = true;
        yield return new WaitForSeconds(1f);
        UnpauseGame();
        yield return null;

    }
    private void PauseGame()
    {
        gameList[activeGameIndex].Pause();
    }
    private void UnpauseGame()
    {
        if(gameState==GameState.PAUSED)
        {
            gameState = GameState.PLAY;
        }
        gameList[activeGameIndex].Unpause();
    }

    public void LoseLife()
    {
        audioSource.PlayOneShot(deathClip);
        lives--;
        if(lives<=0)
        {
            UpdateLives(false);
            GameOver(false);
        }
        else
        {
            UpdateLives(false);
        }
    }

    public void GainLife()
    {
        if (lives >= 5) return;
        lives++;
        UpdateLives(true);
    }

    private void UpdateLives(bool isAddition)
    {
        uIManager.UpdateLives(isAddition);
    }

    public void PlaySpawnPUpClip()
    {
        audioSource.PlayOneShot(spawnPUpClip);
    }
}

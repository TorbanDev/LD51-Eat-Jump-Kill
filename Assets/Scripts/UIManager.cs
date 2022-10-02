using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameManager gm;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI shooterPowerUpText;
    public TextMeshProUGUI snakePowerUpText;
    public TextMeshProUGUI jumpPowerUpText;
    public TextMeshProUGUI gameOverText;
    public List<GameObject> hearts;
    public GameObject buttons;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GameOver(bool win)
    {
        string text = "YOU ";
        if (win) text += "WIN!";
        else text+="LOSE!";

        gameOverText.SetText(text);
        gameOverText.gameObject.SetActive(true);
        buttons.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        timer = gm.gameTimer;
        timerText.SetText(timer.ToString("#.##"));
    }

    public void UpdateLives(bool isAddition)
    {
        if (isAddition)
        {
            foreach (GameObject h in hearts.ToArray())
            {
                if (h.activeSelf==false)
                {
                    h.SetActive(true);
                    break;
                }
            }
        }


            else
            {
                foreach (GameObject h in hearts.ToArray())
                {
                    if (h.activeSelf)
                    {
                        h.SetActive(false);
                        break;
                    }
                }
            }

    }

    public void UpdateShooterPowerUpUI(int count)
    {
        shooterPowerUpText.SetText("x" + count.ToString());
    }
    public void UpdateSnakePowerUpUI(int count)
    {
        snakePowerUpText.SetText("x" + count.ToString());
    }

    internal void UpdateJumpPowerUpUI(int count)
    {
        jumpPowerUpText.SetText("x" + count.ToString());
    }
}

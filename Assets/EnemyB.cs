using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB : Enemy
{
    public SO_GAME game;
    float speed;

    float disableTimer;
    float timerMax = 8f;


    // Start is called before the first frame update
    void Start()
    {
        spawnWeight = 100;
        speed = UnityEngine.Random.Range(6f, 11f);
        disableTimer = timerMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            disableTimer -= Time.deltaTime;
            if(disableTimer<=0)
            {
                disableTimer = timerMax;
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (game.paused) return;
        else
        {
            transform.position += (Vector3.left * Time.fixedDeltaTime * speed);
        }
    }
    public override void Disable(bool awardPoints)
    {
        // Dont call disable from getting hit
    }
    private void RealDisable()
    {
        gameObject.SetActive(false);
    }
}

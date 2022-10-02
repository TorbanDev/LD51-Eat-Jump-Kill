using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    public SO_GAME game;
    public ShooterGame gm;
    public GameObject projectile;
    public ShooterPlayer player;
    float shootDelay =2f;
    int shootMax=2;
    float shootTimer;
    float speed = 22f;
    float targetX;

    float diveTimer;

    EnemyProj proj;
    Vector3 targetPos;
    Vector3 moveDir;
    SpriteRenderer sr;
    int shotCount = 0;
    Color defaultColor;
    Color white = Color.white;

    enum State {SHOOTING, DIVING, SPAWNING};
    State myState = State.SPAWNING;
    // Start is called before the first frame update
    void Start()
    {
        spawnWeight = 100;
        shootTimer = shootDelay;
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        proj = projectile.GetComponent<EnemyProj>();
        targetX = UnityEngine.Random.Range(4, 7);
        diveTimer = shootDelay;
    }

    private void OnEnable()
    {
        myState = State.SPAWNING;
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
        if(sr.color==Color.white)
        {
            sr.color = defaultColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            if (transform.position.x <= -10) Disable(false);
            switch(myState)
            {
                case State.SHOOTING:
                    {
                        shootTimer -= Time.deltaTime;
                        if (shootTimer <= 0)
                        {
                            shootTimer = shootDelay;
                            if (shotCount >= shootMax)
                            {
                                StartCoroutine(StartDive());
                            }
                            else
                            {
                                StartCoroutine(StartShoot());
                            }
                        }
                        break;
                    }
                case State.DIVING:
                    {
                        diveTimer -= Time.deltaTime;
                        if (diveTimer <= 0)
                        {
                            Disable(false);
                        }
                        break;
                    }
                case State.SPAWNING:
                    {
                        break;
                    }
                default: break;
            }

        }
    }
    private void FixedUpdate()
    {
        if (game.paused) return;
        else
        {
            switch (myState)
            {
                case State.SHOOTING:
                    {

                        break;
                    }
                case State.DIVING:
                    {
                        transform.position += (moveDir * Time.deltaTime * speed);
                        break;
                    }
                case State.SPAWNING:
                    {
                        if (transform.position.x <= targetX)
                        {
                            myState = State.SHOOTING;
                        }
                        else
                        {
                            transform.position += (Vector3.left * Time.fixedDeltaTime * (speed/4));
                        }
                        break;
                    }
                default: break;
            }
        }
    }

    IEnumerator StartShoot()
    {
        bool cycle = false;
        for (int i=0; i<=5; i++)
        {
            if (cycle)
            {
                sr.color = white;
                cycle = !cycle;
            }
            else
            {
                sr.color = defaultColor;
                cycle = !cycle;
            }
            yield return new WaitForSeconds(.1f);
        }
        sr.color = defaultColor;
        yield return new WaitForSeconds(.25f);
        Shoot();
        shotCount++;
    }
    void Shoot()
    {
        targetPos = GetTargetPos();
        proj.target = targetPos;
        projectile.transform.localPosition = transform.localPosition;
        projectile.SetActive(true);
        
        // Dont need to pool this. Just use the same obj.

    }
    Vector3 GetTargetPos()
    {
        return player.transform.position;
    }
    IEnumerator StartDive()
    {
        bool cycle = false;
        for (int i = 0; i <= 4; i++)
        {
            if (cycle)
            {
                sr.color = white;
                cycle = !cycle;
            }
            else
            {
                sr.color = defaultColor;
                cycle = !cycle;
            }
            yield return new WaitForSeconds(.1f);
        }
        for (int i = 0; i <= 4; i++)
        {
            if (cycle)
            {
                sr.color = white;
                cycle = !cycle;
            }
            else
            {
                sr.color = defaultColor;
                cycle = !cycle;
            }
            yield return new WaitForSeconds(.05f);
        }
        for (int i = 0; i <= 4; i++)
        {
            if (cycle)
            {
                sr.color = white;
                cycle = !cycle;
            }
            else
            {
                sr.color = defaultColor;
                cycle = !cycle;
            }
            yield return new WaitForSeconds(.025f);
        }
        sr.color = defaultColor;
        yield return new WaitForSeconds(.2f);
        Dive();
        yield return null;
    }

    private void Dive()
    {
        targetPos = GetTargetPos();
        moveDir = (targetPos - transform.position).normalized;
        myState = State.DIVING;

    }
    public override void Disable(bool awardPoints)
    {
        gameObject.SetActive(false);
        myState = State.SPAWNING;
        diveTimer = shootDelay;
        shotCount = 0;
        if(awardPoints)
        {
            gm.SpawnPowerUp(transform.position);
        }
    }
}

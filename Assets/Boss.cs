using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 15;

    public Transform retreatPos;
    public int attackMode;
    public SpriteRenderer sr;
    public SO_GAME game;
    public ShooterGame gm;
    public ShooterPlayer player;
    private Vector3 moveDir;
    [SerializeField]
     public Vector3 moveTarget;
    Vector3 distance;
    float fDistance;

    float moveSpeed;
    float moveSpeedMin = 2f;
    float moveSpeedMax = 10f;

    [SerializeField]
    float idleTimer;
    float idleTimerMin = 1f;
    float idleTimerMax = 2f;

    float minMoveX = 5;
    float maxMoveX=7;
    float minMoveY=2.5f;
    float maxMoveY=-2.5f;

    [SerializeField]
    public float attackTimer;
    float attackMin = 2.5f;
 

    public Transform firePoint;

    bool attacking = false;

    public List<GameObject> wave1list;
    public List<GameObject> wave2list;
    public List<GameObject> wave3list;

    public void TakeDamage()
    {
        health--;
        if(health<=0)
        {
            gm.GameOver(true);
        }
    }

    public enum State {IDLE,MOVING,ATTACKING,RETREATING}
    public State myState = State.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        ResetIdleTimer();
        ResetMoveSpeed();
        ResetAttackTimer();
    }

    private float GetRandom(float min, float max)
    {
        return UnityEngine.Random.Range(min,max);
    }
    private void ResetAttackTimer()
    {
        attackTimer = attackMin;
    }
    private void ResetIdleTimer()
    {
        idleTimer = GetRandom(idleTimerMin, idleTimerMax);
    }
    private void ResetMoveSpeed()
    {
        moveSpeed = GetRandom(moveSpeedMin, moveSpeedMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer<=0)
        {
            ResetAttackTimer();
            if(!attacking)
            {
                attacking = true;
                StartAttack();
            }
        }
     
        {
            switch (myState)
            {
                case State.IDLE:
                    {
                        idleTimer -= Time.deltaTime;
                        if(idleTimer<=0)
                        {
                            DoneIdling();
                            ResetIdleTimer();
                        }
                        break;
                    }
                case State.MOVING:
                    {
                        fDistance = Vector3.Distance(moveTarget, transform.position);
                        if (fDistance >= .1f)
                        {
                            moveDir = (moveTarget - transform.position).normalized;
                            transform.position += (moveDir * Time.deltaTime * moveSpeed);
                        }
                        else
                        {
                            myState = State.IDLE;
                            ResetIdleTimer();
                        }
                        break;
                    }
                default: break;
            }
        }
    }

    private void StartAttack()
    {
        if (!attacking) return;
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        attacking = true;
        
        if (attackMode == 1)
        {
            //telegraph
            bool change = true;
            for (int i = 0; i <= 4; i++)
            {
                if (change)
                {
                    sr.color = Color.cyan;
                    yield return new WaitForSeconds(.1f);
                    change = !change;
                }
                else
                {

                    sr.color = Color.white;
                    yield return new WaitForSeconds(.1f);
                    change = !change;
                }
                sr.color = Color.white;
            }
            Debug.Log("SPREAD ATTACK");
            int count = 0;
            int max = wave1list.Count;
            //spread attack
            foreach (GameObject obj in wave1list)
            {
                EnemyProj proj = obj.GetComponent<EnemyProj>();
                proj.target = new Vector3(-1, player.transform.position.y - 1 + count);
                obj.transform.position = firePoint.position;
                obj.SetActive(true);
                count++;
            }
            yield return new WaitForSeconds(.4f);
            count = 0;
            max = wave2list.Count;

            foreach (GameObject obj in wave2list)
            {
                EnemyProj proj = obj.GetComponent<EnemyProj>();
                proj.target = new Vector3(-1, player.transform.position.y - 1 + count);
                obj.transform.position = firePoint.position;
                obj.SetActive(true);
                count++;
            }
            yield return new WaitForSeconds(.4f);
            count = 0;
            max = wave3list.Count;

            foreach (GameObject obj in wave3list)
            {
                EnemyProj proj = obj.GetComponent<EnemyProj>();
                proj.target = new Vector3(-1, player.transform.position.y - 2 + count);
                obj.transform.position = firePoint.position;
                obj.SetActive(true);
                count++;
            }
        }
        else
        {
            attackTimer = .5f;
            moveTarget = retreatPos.position;
            moveSpeed = moveSpeedMax;
            myState = State.MOVING;
            yield return new WaitForSeconds(.3f);
            gameObject.SetActive(false);
            
            
            Debug.Log("SPAWN ATTACK");
            // spawn attack
        }
        ResetAttackTimer();
        attacking = false;
        yield return null;
    }

    private void DoneIdling()
    {
        myState = State.MOVING;
        GetMovePosition();
    }

    public void GetMovePosition()
    {
        float x = GetRandom(minMoveX, maxMoveX);
        float y = GetRandom(minMoveY, maxMoveY);
        moveTarget = new Vector3(x, y, 0);
        moveDir = (moveTarget - transform.position).normalized;
        ResetMoveSpeed();
    }
}

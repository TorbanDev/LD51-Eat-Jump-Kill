using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGame : Minigame
{
    public GameManager gm;
    // Timer
    float timer;
    public float timerMin;
    public float timerMax;

    


    // Pooling
    public GameObject rafterPrefab;
    public GameObject presentPrefab;
    public List<GameObject> rafterPool = new List<GameObject>();
    public List<GameObject> presentPool = new List<GameObject>();
    public int poolSize;

    public JumpPlayer player;

    public AudioSource audioSource;


    void Start()
    {
        Pause();
        rafterPool=CreatePool(rafterPrefab,poolSize);
        presentPool = CreatePool(presentPrefab, 10);
        audioSource = GetComponent<AudioSource>();
    }

    private List<GameObject> CreatePool(GameObject prefab,int poolSize)
    {
        List<GameObject> tempList = new List<GameObject>();
       for (int i=0;i<poolSize;i++)
        {
            GameObject obj = Instantiate(prefab, transform.parent);
            obj.SetActive(false);
            tempList.Add(obj);
        }
        return tempList;
    }

    public GameObject GetRafter()
    {
        for(int i=0;i<poolSize;i++)
        {
            if(rafterPool[i].activeSelf==false)
            {
                return rafterPool[i];
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = SetNewTimer();
                SpawnRafter();
            }
        }
    }

    private void SpawnRafter()
    {
        GameObject obj = GetRafter();
        Rafter rafter = obj.GetComponent<Rafter>();
        rafter.SetFallSpeed();
        rafter.SetSize();
        rafter.SetPosition();
        if(gm.JumpPowerUps>0)
        {
            gm.ChangePowerUp("JUMP_POWERUP", -1);
            GameObject present = GetObject(presentPool);
            JumpPup pUp= present.GetComponent<JumpPup>();
            pUp.Initialize(obj);
            rafter.fallSpeed = rafter.fallSpeedMin;
            gm.PlaySpawnPUpClip();
        }
        obj.SetActive(true);
        
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
    private float SetNewTimer()
    {
        return UnityEngine.Random.Range(timerMin, timerMax);
    }
    public override void Pause()
    {
        player.PauseMovement();
        audioSource.Pause();
        base.Pause();
    }
    public override void Unpause()
    {
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.UnPause();
        player.ResumeMovement();
        base.Unpause();

    }
    public void LoseLife()
    {
        gm.LoseLife();
    }

  
}

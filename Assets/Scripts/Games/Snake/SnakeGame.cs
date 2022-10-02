using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGame : Minigame
{
    AudioSource audioSource;
    public GameManager gm;
    float timer;
    float timerMax=.2f;
    public int snakeLength = 3;
    public List<Cell> cells = new List<Cell>();
    public Cell headCell;
    Vector2 movement;
    Vector2 lastMovement=Vector2.right;
    Vector2 bodyPos=Vector2.left;
    float horizontal;
    float vertical;
    bool skipThisTick;

    public List<Cell> startingCells;
    public Cell startingHead;
    public Cell body1;
    public Cell body2;
    public Cell startingFoodCell;
    Cell mainFoodCell;



    bool respawning = false;

    public GameObject mainFood;
    public GameObject otherFood;
    public GameObject powerupPrefab;
    public Transform foodSpawnPoint;

    public List<GameObject> foodPool = new List<GameObject>();
    public List<GameObject> powerupPool = new List<GameObject>();
    Vector3 mainFoodStartingPosition;
    


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        game.paused = true;
        startingCells = cells;
        mainFoodStartingPosition = mainFood.transform.localPosition;
        mainFoodCell = startingFoodCell;
        foodPool = CreatePool(otherFood);
        powerupPool = CreatePool(powerupPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Tick();
                timer = timerMax;
            }
            if(lastMovement.x==-1||lastMovement.x==1)
            {
                movement.y = Input.GetAxisRaw("Vertical");
                movement.x = 0;
            }
            if (lastMovement.y == -1 || lastMovement.y == 1)
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = 0;
            }
                
            
        }
       
        
    }

    void Tick()
    {
        if (respawning) return;
        List<Cell> tempList = cells;
        foreach(Cell c in tempList.ToArray())
        {
            c.Tock(skipThisTick);
        }
        skipThisTick = false;
        Vector2 moveDir = CalculateDirection();
        lastMovement = moveDir;
        bodyPos = moveDir * -1;
        MoveThisWay(moveDir);
    }
    private List<GameObject> CreatePool(GameObject prefab)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(prefab, transform.parent);
            obj.SetActive(false);
            list.Add(obj);
        }
        return list;
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
    private void MoveThisWay(Vector3 moveDir)
    {
        Collider2D col = Physics2D.OverlapCircle(headCell.transform.position + moveDir, .15f);
        if (col != null)
        {
            if(col.CompareTag("Respawn"))
            {
                Respawn();
                return;
            }
            Cell newCell= col.gameObject.GetComponent<Cell>();
            if (newCell != null)
            {
                if (newCell.isActive)
                {
                    Respawn();
                    return;
                }
                newCell.Activate();
                headCell = newCell;
                if (newCell.hasMainFood)
                {
                    snakeLength++;
                    skipThisTick = true;
                    newCell.hasMainFood = false;
                    MoveMainFood();
                    CollectPowerUp(newCell.transform.position);
                }
                if (newCell.hasOtherFood)
                {
                    if (snakeLength > 1)
                    {
                        snakeLength++;
                    }
                    newCell.RemoveOtherFood();
                    CollectPowerUp(newCell.transform.position);
                }
            }

        }
    }

    void CollectPowerUp(Vector3 spawnPoint)
    {
        GameObject obj = GetObject(powerupPool);
        obj.transform.position = spawnPoint;
        obj.SetActive(true);
    }

    private void Respawn()
    {
        gm.LoseLife();
        StartCoroutine(StartRespawn());
    }

    IEnumerator StartRespawn()
    {
        respawning = true;
        for(int i =0; i < cells.Count;i++)
        {
            cells[i].TurnWhite();
        }
        yield return new WaitForSeconds(.125f);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].TurnRed();
        }
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].TurnWhite();
        }
        yield return new WaitForSeconds(.125f);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].TurnRed();
        }

        yield return new WaitForSeconds(1f);
        Reset();
        yield return new WaitForSeconds(.5f);
        Resume();
        yield return null;
    }

    private void Resume()
    {
        timer = timerMax;
        respawning = false;
    }

    private void Reset()
    {
        List<Cell> tmpList = cells;
        foreach(Cell c in tmpList.ToArray())
        { 
            c.Deactivate();
        }

        mainFoodCell.hasMainFood = false;
        mainFoodCell = startingFoodCell;

        snakeLength = 3;

        headCell = startingHead;
        startingHead.Activate();
        body1.TurnToBody();
        body1.count = 2;
        AddCellToList(body1);
        body2.TurnToBody();
        body2.count = 1;
        AddCellToList(body2);


        lastMovement = Vector2.right;
        bodyPos = Vector2.left;

        mainFood.transform.localPosition = mainFoodStartingPosition;
        startingFoodCell.hasMainFood = true;
    }

    private void MoveMainFood()
    {
        Cell newCell = GetNewCell();
        if (newCell == null) return;
        mainFood.transform.position = newCell.transform.position;
        newCell.hasMainFood = true;
        mainFoodCell = newCell;
    }

    private Cell GetNewCell()
    {
        Cell cell = null;
        do { cell = GetNewCellPrivate(); }
        while (cell == null);
        return cell;
    }
    private Cell GetNewCellPrivate()
    {
        Cell newCell=null;
        float x = (float) (Math.Round(UnityEngine.Random.Range(-7.5f, 7.5f)) * .5f) / 0.5f;
        float y = (float) (Math.Round(UnityEngine.Random.Range(-2.5f, 3.5f)) * .5f) / 0.5f;
        Vector2 pos = new Vector2(x, y);
        Collider2D col = Physics2D.OverlapCircle(pos, .125f);
        if(col!=null)
        {
            newCell = col.GetComponent<Cell>();
            if(newCell==null)
            {
                newCell=GetNewCell();
            }
            if (newCell.hasOtherFood||newCell.hasMainFood||newCell.isActive)
            {
                newCell=GetNewCell();
            }
            else return newCell;
        }
        if (newCell == null) return GetNewCell();
        return null;
    }

    private Vector2 CalculateDirection()
    {
        Vector2 moveDir;
        if (movement.x == 0 && movement.y == 0)
        {
            moveDir = lastMovement;
        }
        else if (movement == bodyPos)
        {
            moveDir = lastMovement;
        }
        else moveDir = movement;
        return moveDir;
    }

    public void AddCellToList(Cell cell)
    {
        cells.Add(cell);
    }
    public void RemoveCellFromList(Cell cell)
    {
        cells.Remove(cell);
    }
    public override void Unpause()
    {
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.UnPause();
        LaunchPowerUps();
        base.Unpause();
    }
    public override void Pause()
    {
        audioSource.Pause();
        base.Pause();
    }

    private void LaunchPowerUps()
    {
        StartCoroutine(StartLaunchPowerUps());
    }
    IEnumerator StartLaunchPowerUps()
    {
        int pCount = gm.ShooterPowerUps;
        if (pCount>0)
        {
            yield return new WaitForSeconds(.2f);
            for(int i =0;i<pCount;i++)
            {
                GameObject obj = GetObject(foodPool);
                if (obj == null) obj = GetObject(foodPool);
                obj.transform.position = foodSpawnPoint.position;

                OtherFood food = obj.GetComponent<OtherFood>();

                Cell cell = GetNewCell();
                cell.AddOtherFood(obj);
                food.target = cell.transform.position;

                obj.SetActive(true);
                gm.ChangeShooterPowerUp(-1);
                gm.PlaySpawnPUpClip();
                yield return new WaitForSeconds(.2f);
            }
        }
        yield return null;
    }
}

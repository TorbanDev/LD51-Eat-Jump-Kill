using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShooterGame : Minigame
{
    AudioSource audioSource;
    public ParticleSystem ps;

    public Boss boss;

    public GameObject launchPrefab;
    public GameObject rocketPrefab;
    public GameObject laserPrefab;
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;

    public GameObject shield;

    public ShooterPlayer player;
    public GameManager gm;

    public List<GameObject> enemyAPool;
    public List<GameObject> enemyBPool;
    public List<GameObject> launchPool;
    public List<GameObject> rocketPool;

    public int waveCount = 0;

    public Transform firePoint;

    // Enemy Wave Management
    float waveTimer;
    public float waveCooldown = 12f;
    public int waveSizeMin = 3;
    public int waveSizeMax = 6;
    int poolWeight;
    public List<Enemy> spawnableEnemies = new List<Enemy>();
    SortedList<int, Enemy> enemyWeightList = new SortedList<int, Enemy>();

    // Powerups
    public GameObject powerUpPrefab;
    public List<GameObject> powerUpPool = new List<GameObject>();
    public List<GameObject> UiUpgradePool;
    public GameObject upgradePrefab;
    public List<GameObject> upgradePool;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        game.paused = true;
        launchPool = CreatePool(launchPrefab);
        enemyAPool = CreatePool(enemyAPrefab);
        enemyBPool = CreatePool(enemyBPrefab);
        //rocketPool = CreatePool(rocketPrefab);

        powerUpPool = CreatePool(powerUpPrefab);
        upgradePool = CreatePool(upgradePrefab);

        int temp = 0;
        foreach(Enemy e in spawnableEnemies)
        {
            temp += e.spawnWeight;
            enemyWeightList.Add(temp, e);
        }
    }


    public void SpawnWave()
    {
        StartCoroutine(StartSpawn());
    }

    private int GetPoolWeight()
    {
        int totalWeight=0;
        foreach(Enemy e in spawnableEnemies)
        {
            totalWeight += e.spawnWeight;
        }
        return totalWeight;
    }

    IEnumerator StartSpawn()
    {
        // Loop through spawnableenemies list
        // Get the total weight
        PrepSpawnTable();
        

        int numToSpawn = UnityEngine.Random.Range(waveSizeMin, waveSizeMax + 1);
        for (int i = 0; i < numToSpawn; i++)
        {

            if (game.paused) continue;
            // Get a random number. min=1, max= total weight
            int roll = UnityEngine.Random.Range(1, poolWeight);
            Enemy e = null;
            foreach(var p in enemyWeightList)
            {
                if (roll<p.Key)
                {
                    e = p.Value;
                    break;
                }
                else
                {

                }
            }

            // Run number through function which returns enemy type based on weight.
            // Spawn that enemy
            GameObject obj = GetEnemyFromPool(e);

            obj.transform.position = new Vector3(8, UnityEngine.Random.Range(-2.5f, 3.5f), 0);
            obj.SetActive(true);
            yield return new WaitForSeconds(UnityEngine.Random.Range(.5f, 1.5f));
        }
        yield return null;
    }

    private GameObject GetEnemyFromPool(Enemy e)
    {
        if(e.gameObject.TryGetComponent(out EnemyA ea))
        {
            return GetObject(enemyAPool);
        }
        if (e.gameObject.TryGetComponent(out EnemyB eb))
        {
            return GetObject(enemyBPool);
        }
        return null;
    }

    private void PrepSpawnTable()
    {
        poolWeight = GetPoolWeight();
    }

    public void Launch()
    {
        GameObject obj = GetObject(launchPool);
        obj.transform.position = firePoint.position;
        obj.SetActive(true);

    }

    private List<GameObject> CreatePool(GameObject prefab)
    {
        List<GameObject> list=new List<GameObject>();
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

    public void PlayerHit()
    {
        if (player.respawning) return;
        if(shield.activeSelf)
        {
            DestroyShield();
        }
        else
        {
            gm.LoseLife();
            Respawn();
        }
    }

    private void Respawn()
    {
        
        StartCoroutine(Respawning());
    }
    IEnumerator Respawning()
    {
        player.respawning = true;
        player.TakeHit();
        yield return new WaitForSeconds(1.5f);
        player.respawning = false;
        yield return null;
    }
    private void DestroyShield()
    {
        // Particle effect for shield break
        shield.SetActive(false);
    }

    public void GainShield()
    {
        if (shield.activeSelf==false)
        {
            shield.SetActive(true);
        }
    }

    public override void CreatePowerUp()
    {
        
    }

    public void SpawnPowerUp(Vector3 spawnPoint)
    {
        GameObject obj = GetObject(powerUpPool);
        {
            obj.transform.position = spawnPoint;
            obj.SetActive(true);
        } 
    }

    public override void Pause()
    {
        audioSource.Pause();
        ps.Pause();
        base.Pause();
    }
    public override void Unpause()
    {
        waveCount++;
        if (!audioSource.isPlaying) audioSource.Play();
        audioSource.UnPause();
        SpawnUpgrades();
        ps.Play();
        Debug.Log("UNPAUSING");
        base.Unpause();
        if (waveCount % 2 == 0)
        {
            Debug.Log("MODULO");
            boss.attackMode = 1;
            boss.gameObject.SetActive(true);
            boss.GetMovePosition();
            boss.myState = Boss.State.MOVING;
        }
        else
        {
            Debug.Log("SPAWNING");
            boss.attackTimer = .5f;
            boss.attackMode = 0;
            SpawnWave();
        }
        base.Unpause();
    }
    public void GameOver(bool win)
    {
        gm.GameOver(win);
    }
    private void SpawnUpgrades()
    {
        StartCoroutine(StartSpawnUpgrades());
    }
    IEnumerator StartSpawnUpgrades()
    {

        foreach (GameObject go in UiUpgradePool.ToArray())
        {
            if(go.activeSelf)
            {
                GameObject obj = GetObject(upgradePool);
                Upgrade up = obj.GetComponent<Upgrade>();
                up.Initialize();
                UIUpgrade uiUpgrade = go.GetComponent<UIUpgrade>();
                obj.GetComponent<SpriteRenderer>().sprite = uiUpgrade.GetComponentInChildren<Image>().sprite;
                yield return new WaitForSeconds(.1f);
                GetUpgrade(uiUpgrade.pName);
                go.SetActive(false);
            }
        }
        yield return null;
    }
    public void GetUpgrade(string type)
    {
        switch (type)
        {
            case "SHIELD":
                AddShield();
                break;
            case "LASER":
                AddLaser();
                break;
            case "ROCKET":
                AddRocket();
                break;
            default: break;
        }
    }

    private void AddRocket()
    {
        Debug.Log("ADDING ROCKET! NOT YET IMPLEMENTED ");
    }

    private void AddLaser()
    {
        if (player.weaponCooldownMax>=.3)
        {
            player.weaponCooldownMax -= .2f;
        }
        else
        {
            // TO DO: UPGRADE TO LASER BIZZEAM;
        }
    }

    private void AddShield()
    {
        GainShield();
    }
}

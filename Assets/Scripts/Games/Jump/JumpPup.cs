using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPup : MonoBehaviour
{
    public enum pUpType { HEALTH,SHIELD,LASER,ROCKET};
    public enum state { IDLE, COLLECTED};
    public state myState = state.IDLE;
    public pUpType myType;

    public JumpGame jgm;
    public GameManager gm;

    public SO_POWERUP pUp;

    public SpriteRenderer sr;

    public Vector3 collectedTarget;
    Vector3 moveDir;
    Vector3 distance;
    float fDistance;
    float speed = 18f;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TRIGGERED");
        if(collision.TryGetComponent(out JumpPlayer player)) {
            Debug.Log("FOUND PLAYER");
            myState = state.COLLECTED;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myState == state.IDLE) return;
        else
        {
            fDistance = Vector3.Distance(collectedTarget, transform.position);
            if (fDistance >= 1f)
            {
                moveDir = (collectedTarget - transform.position).normalized;
                transform.position += (moveDir * Time.deltaTime * speed);
            }
            else
            {
                AddPowerUp(myType);
                Disable();
            }
        }
    }

    void AddPowerUp(pUpType type)
    {
        string pName = GetTypeName(type);
        gm.AddUpgrade(pName);
    }

    public void Initialize(GameObject newParent)
    {
        myType = GetMyType();
        string pName = GetTypeName(myType);
        sr.sprite = pUp.GetSprite(pName);
        transform.SetParent(newParent.transform);
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(0, .4f, 0);
    }

    private string GetTypeName(pUpType type)
    {
        string pName = null;
        switch (type)
        {
            case pUpType.HEALTH:
                pName = "HEALTH";
                break;
            case pUpType.SHIELD:
                pName = "SHIELD";
                break;
            case pUpType.LASER:
                pName = "LASER";
                break;
            case pUpType.ROCKET:
                pName = "ROCKET";
                break;
            default: break;
        }
        return pName;
    }

    private pUpType GetMyType()
    {
        float roll = UnityEngine.Random.Range(0f, .75f); // TO DO - change back to 100.
        // TO DO - Add rockets
        if (roll < .25f)
        {
            return pUpType.HEALTH;
        }
        if (roll <.5f)
        {
            return pUpType.SHIELD;
        }
        if (roll <.75f)
        {
            return pUpType.LASER;
        }
        else
        {
            return pUpType.LASER;
        }
    }
    public void Disable()
    {
        myState = state.IDLE;
        transform.SetParent(jgm.transform.parent);
        gameObject.SetActive(false);
    }
}

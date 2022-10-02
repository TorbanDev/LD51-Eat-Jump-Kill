using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafter : MonoBehaviour
{
    public SO_GAME game;
    Rigidbody2D rb;
    public BoxCollider2D bc;

    // Speed
    public float fallSpeed = 2f;
    public float fallSpeedMin = 2f;
    float fallSpeedMax = 4f;

    float sizeMin = .08f;
    float sizeMax = .3f;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void SetSize()
    {
        float x= UnityEngine.Random.Range(sizeMin, sizeMax);
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    public void SetFallSpeed()
    {
        fallSpeed = UnityEngine.Random.Range(fallSpeedMin, fallSpeedMax);
    }
    public void SetPosition()
    {
        float x = UnityEngine.Random.Range(-.4f, .4f);
        transform.localPosition=(new Vector3(x, -.5f, transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (game.paused) return;
        else
        {
            rb.MovePosition((Vector3.down * fallSpeed * Time.fixedDeltaTime) + transform.position);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            Disable();
            
        }
        else
        {
            JumpPlayer jp = collision.GetComponentInParent<JumpPlayer>();
            if (jp != null)
            {
                if (jp.shouldTriggerRafters)
                {
                    TriggerRafterCollider();
                    jp.Grounded();
                }
            }
        }

    }

    private void Disable()
    {
        if (transform.childCount > 0)
        {
            JumpPup pup = GetComponentInChildren<JumpPup>();
            pup.Disable();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn")==false)
        {
            JumpPlayer jp = collision.GetComponentInParent<JumpPlayer>();
            if (jp != null)
            {
                bc.enabled = false;
            }
        }
    }

    private void TriggerRafterCollider()
    {
        bc.enabled = true;
    }
    private void OnDisable()
    {
        bc.enabled = false;
    }
}

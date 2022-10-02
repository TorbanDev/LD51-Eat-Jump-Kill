using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayer : MonoBehaviour
{
    public SO_GAME game;
    private Rigidbody2D rb;
    public ShooterGame gm;
    Vector3 startingPosition;

    SpriteRenderer sr;
    public bool respawning = false;

    // movement
    public float speed = 5f;
    Vector3 movement;

    // weapons
    public float weaponCooldownMax=1.5f;
    float weaponCooldownTimer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        startingPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            // movement
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            weaponCooldownTimer -= Time.deltaTime;
            if (weaponCooldownTimer <= 0)
            {
                weaponCooldownTimer = weaponCooldownMax;
                Launch();
            }
        }


    }

    private void Launch()
    {
        gm.Launch();
    }

    private void FixedUpdate()
    {
        if (game.paused) return;
        rb.MovePosition(transform.position + movement * Time.fixedDeltaTime * speed);
    }

    public void TakeHit()
    {
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        Color currentCol = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentCol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.3f);
        sr.color = currentCol;
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.TryGetComponent(out Enemy enemy))
        {
            gm.PlayerHit();
        }
    }

}

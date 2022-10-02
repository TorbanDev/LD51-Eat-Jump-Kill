using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayer : MonoBehaviour
{
    public JumpGame gm;
    public SO_GAME game;
    public Rigidbody2D rb;
    public bool shouldTriggerRafters = true;
    public bool respawning = false;
    public Transform respawnPoint;

    SpriteRenderer sr;

    Vector3 movement;
    bool jumped = false;
    public float jumpForce = 4f;
    [SerializeField]
    private bool canJump = false;
    public float speed=8f;

    bool died = false;

    Vector2 previousVelocity;
    Vector2 nullVelocity = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.paused) return;
        else
        {
            if (respawning)
            {

                return;
            }
            else
            {
                if (rb.velocity.y >= 0)
                {
                    shouldTriggerRafters = false;
                }
                else
                {
                    shouldTriggerRafters = true;
                }
                movement.x = Input.GetAxisRaw("Horizontal");
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumped = true;
                }
            }

        }
    }
    private void FixedUpdate()
    {
        if (game.paused) return;
        else
        {
            
            if (jumped && canJump)
            {
                jumped = false;
                canJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
            jumped = false;
            if (respawning)
            {
                Vector3 distance = (respawnPoint.position - transform.position);
                float relativeDistance = Mathf.Abs(distance.y);
                if (relativeDistance <= .2f)
                {
                    respawning = false;
                    rb.velocity = nullVelocity;
                    canJump = true;
                }
                else
                {
                    rb.velocity = nullVelocity;
                    Vector3 dir = distance.normalized;
                    transform.position += dir * Time.deltaTime * 20f;
                }
                died = false;
            }
            else transform.position += movement * Time.fixedDeltaTime * speed;
            
        }

    }
    public void AddForce()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
    public void Grounded()
    {
        Debug.Log("GROUNDED");
        canJump = true;
    }

    public void PauseMovement()
    {
        previousVelocity = rb.velocity == null ? new Vector2(0, 0) : rb.velocity ;
        rb.velocity = nullVelocity;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ResumeMovement()
    {
        rb.velocity = previousVelocity;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Respawn"))
        {
            Respawn();

        }
    }
    void Respawn()
    {
        if (!died)
        {
            gm.LoseLife();
            died = true;
        }
        
        StartCoroutine(StartRespawn());

    }
    IEnumerator StartRespawn()
    {
        Color currentcol = sr.color;
        PauseMovement();
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentcol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = currentcol;
        yield return new WaitForSeconds(.1f);
        sr.color = Color.red;
        ResumeMovement();
        respawning = true;
        yield return new WaitForSeconds(.2f);
        sr.color = currentcol;
    }
}

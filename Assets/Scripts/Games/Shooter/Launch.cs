using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launch : MonoBehaviour
{
    public float speed = 19f;
    public ShooterGame gm;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (gm.game.paused) return;
        else
        {
            transform.position += (Vector3.right * Time.fixedDeltaTime * speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            boss.TakeDamage();
            gameObject.SetActive(false);
        }
        if(collision.CompareTag("Respawn"))
        {
            Deactivate();   
        }
        if (collision.TryGetComponent(out Enemy enemy))
        {
            enemy.Disable(true);

            if (collision.CompareTag("Enemy"))
            {
                gameObject.SetActive(false);
            }
            // Play particle effect
        }
    }

    void Deactivate()
    {
        StartCoroutine(Disable());
    }
    IEnumerator Disable()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
        yield return null;
    }
}

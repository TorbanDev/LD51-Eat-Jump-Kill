using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProj : Enemy
{
    public SO_GAME game;
    public Vector3 target;
    public float speed =14f;
    Vector3 dir;
    // Start is called before the first frame update
    void OnEnable()
    {
        dir = (target - transform.position).normalized;
        Invoke("Disable", 1.5f);
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
            transform.position += (Time.fixedDeltaTime * dir * speed);
        }
    }
    private void Disable()
    {
        gameObject.SetActive(false);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform target;
    Vector3 moveDir;
    Vector3 distance;
    float fDistance;
    float speed = 18f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fDistance = Vector3.Distance(target.position, transform.position);
        if (fDistance >= 1f)
        {
            moveDir = (target.position - transform.position).normalized;
            transform.position += (moveDir * Time.deltaTime * speed);
        }
        else
        {
            Disable();
        }

    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
    }
}

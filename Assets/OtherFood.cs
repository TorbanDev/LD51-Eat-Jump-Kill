using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherFood : MonoBehaviour
{
    public Vector3 target;
    Vector3 moveDir;
    bool reachedTarget;
    float fDistance;
    // Start is called before the first frame update
    void Start()
    {
        moveDir = (target - transform.position).normalized;
    }
    private void OnEnable()
    {
        moveDir = (target - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if (reachedTarget) return;
        fDistance = Vector3.Distance(target, transform.position);
        if(fDistance<=.02f) { 

            transform.position = target;
            reachedTarget = true;
        }
        else
        {
            moveDir = (target - transform.position).normalized;
            transform.position += (moveDir * Time.deltaTime * 13f);
        }
    }
    private void OnDisable()
    {
        reachedTarget = false;
    }
}

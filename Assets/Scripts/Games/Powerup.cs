using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameManager gm;
    public SO_POWERUP powerup;
    Vector3 moveDir;
    Vector3 distance;
    float fDistance;
    float speed = 18f;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("target: "+powerup.target+". pos: "+transform.position);
        moveDir = (powerup.target - transform.position).normalized;
    }

    private void OnEnable()
    {
        Invoke("CleanUp", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {

        fDistance = Vector3.Distance(powerup.target,transform.position);
        if(fDistance>=1f)
        {
            moveDir = (powerup.target - transform.position).normalized;
            transform.position += (moveDir * Time.deltaTime * speed);
        }
        else
        {
            AddPowerUp(powerup);
            gameObject.SetActive(false);
        }
    }

    void CleanUp()
    {
        if (gameObject.activeSelf)
        {
            AddPowerUp(powerup);
            gameObject.SetActive(false);
        }
    }
    private void AddPowerUp(SO_POWERUP p)
    {
        gm.ChangePowerUp(p.name,1);
    }
}

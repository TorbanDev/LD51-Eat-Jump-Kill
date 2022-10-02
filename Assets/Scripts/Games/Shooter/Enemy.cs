using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int spawnWeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Disable(false);
        }
    }
    public virtual void Disable(bool awardPoints)
    {
        // play particle effect
        gameObject.SetActive(false);
    }
}

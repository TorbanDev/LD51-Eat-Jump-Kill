using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    public SO_GAME game;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    virtual public void Pause()
    {
        game.Pause();
    }
    virtual public void Unpause()
    {
        game.Play();
    }
    virtual public void CreatePowerUp()
    {

    }
}

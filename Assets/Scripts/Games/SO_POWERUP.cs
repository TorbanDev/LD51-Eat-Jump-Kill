using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO", menuName = "ScriptableObjects/SO_POWERUP")]
public class SO_POWERUP : ScriptableObject
{
   
    public Vector3 target;
    public Sprite healthSprite;
    public Sprite shieldSprite;
    public Sprite rocketSprite;
    public Sprite laserSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Sprite GetSprite(string pType)
    {
        switch (pType)
        {
            case "HEALTH":
                return healthSprite;
            case "SHIELD":
                return shieldSprite;
            case "LASER":
                return laserSprite;
            case "ROCKET":
                return rocketSprite;
            default: return null;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgrade : MonoBehaviour
{
    public SO_POWERUP pUp;
    public Image image;
    public enum pUpType { HEALTH, SHIELD, LASER, ROCKET };
    public pUpType myType;
    public string pName;
    // Start is called before the first frame update
    void Start()
    {
        pName = GetTypeName(myType);
        image = GetComponentInChildren<Image>();
    }
    public void Initialize(string type)
    {
        if(image==null)
        {
            image= GetComponentInChildren<Image>();
        }
        myType = GetTypeByName(type);
        pName = type;
        image.sprite = pUp.GetSprite(pName);
        gameObject.SetActive(true);
    }
    public void Disable()
    {
        gameObject.SetActive(false);
    }
    private string GetTypeName(pUpType type)
    {
        string pName = null;
        switch (type)
        {
            case pUpType.HEALTH:
                pName = "HEALTH";
                break;
            case pUpType.SHIELD:
                pName = "SHIELD";
                break;
            case pUpType.LASER:
                pName = "LASER";
                break;
            case pUpType.ROCKET:
                pName = "ROCKET";
                break;
            default: break;
        }
        return pName;
    }
    private pUpType GetTypeByName(string name)
    {
        pUpType pType = pUpType.HEALTH;
        switch (name)
        {
            case "HEALTH":
                pType = pUpType.HEALTH;
                break;
            case "SHIELD":
                pType = pUpType.SHIELD;
                break;
            case "LASER":
                pType = pUpType.LASER;
                break;
            case "ROCKET":
                pType = pUpType.ROCKET;
                break;
            default: break;
        }
        return pType;
    }
}

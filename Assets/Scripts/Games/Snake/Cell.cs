using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int count=0;
    public bool isHead;
    public bool isActive;
    public SnakeGame gm;
    public bool hasMainFood = false;
    public bool hasOtherFood = false;
    SpriteRenderer sr;

    public GameObject otherfood;

    Color darkestGreen = new Color32(15, 56, 15,255);
    Color darkGreen = new Color32(48, 98, 48,255);
    Color lightGreen = new Color32(139, 172, 15,255);
    Color lightestGreen = new Color32(155, 188, 15,255);

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Tock(bool skip)
    {
        
        if (!isActive) return;
        
        if (isHead)
        {
            TurnToBody();
        }
        if (skip) return;
        count -= 1;
        if (count==0)
        {
            Deactivate();
        }
    }
    public void TurnToBody()
    {
        sr.color = lightGreen;
        isHead = false;
        isActive = true;
    }
    public void Deactivate()
    {
        isActive = false;
        sr.color = darkestGreen;
        gm.RemoveCellFromList(this);
    }
    public void Activate()
    {
        count = gm.snakeLength;
        isActive = true;
        isHead = true;
        sr.color = darkGreen;
        gm.AddCellToList(this);
    }
    public void AddOtherFood(GameObject food)
    {
        otherfood = food;
        hasOtherFood = true;
    }
    public void RemoveOtherFood()
    {
        hasOtherFood = false;
        otherfood.SetActive(false);
    }
    public void TurnRed()
    {
        sr.color = Color.red;
    }
    public void TurnWhite()
    {
        sr.color = Color.white;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO", menuName = "ScriptableObjects/SO_Game")]
public class SO_GAME : ScriptableObject
{
    public bool paused = true;

    public int gameIndex;


    public void Pause()
    {
        paused = true;
    }

    public void Play()
    {
        paused = false;
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LevelManager : MonoBehaviour {

    protected static int openingLeaksFixed = 0;
    protected static bool openingFinished = false;

    protected GameManager gameManager;

    protected virtual void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    public virtual void EventLeakFixed(int message)
    {

    }
}

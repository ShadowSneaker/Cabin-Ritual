using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticArcadeInfo 
{
    private int KILLS;
    private float TIME;
    private int SCORE;


    public int kills
    {
        get
        {
            return KILLS;
        }
        set
        {
            KILLS = value;
        }

    }

    public float time
    {
        get
        {
            return TIME;
        }
        set
        {
            TIME = value;
        }
    }

    public int score
    {
        get
        {
            return SCORE;
        }
        set
        {
            SCORE = value;
        }
    }


}

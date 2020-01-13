using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticArcadeInfo 
{
    private static int KILLS;
    private static float TIME;
    private static int SCORE;


    public static int Kills
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

    public static float Time
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

    public static int Score
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

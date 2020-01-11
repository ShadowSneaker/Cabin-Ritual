using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersPoints : MonoBehaviour
{
    public int PointsAquired;
    public int KillCount;



    public void RemovePoints(int points)
    {
        PointsAquired -= points;        
    }

    public void AddPoints(int Points)
    {
        PointsAquired += Points;
    }

    public void AddKill()
    {
        KillCount++; 
    }


    public void EndPoints()
    {
        StaticArcadeInfo.kills = KillCount;
        StaticArcadeInfo.score = PointsAquired;
        StaticArcadeInfo.time = Time.time;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersPoints : MonoBehaviour
{
    public int PointsAquired;

    public void RemovePoints(int points)
    {
        PointsAquired -= points;
    }
}

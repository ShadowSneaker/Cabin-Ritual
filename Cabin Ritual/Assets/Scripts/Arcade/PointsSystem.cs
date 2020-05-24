using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsSystem : MonoBehaviour
{
    // the points that each zombie drops when killed
    public int ZombiePoints = 100;

    // the number of doors the player has opened in arcade mode
    public int ZombiedoorNumber = 0;    

    private PlayersPoints ThePlayer;        

    private void Start()
    {
        ThePlayer = GameObject.Find("Player With Gun").GetComponent<PlayersPoints>();
    }

    // function to add points to the players point system
    public void AddPoints()
    {        
        ThePlayer.PointsAquired += 100;
    }
    public void AddHitPoints()
    {
        ThePlayer.PointsAquired += 10;
    }

    public int GetPlayerPointsAquired()
    {
        return ThePlayer.PointsAquired;
    }

   public PlayersPoints GetPlayerPoints()
   {
       return ThePlayer;
   }
}

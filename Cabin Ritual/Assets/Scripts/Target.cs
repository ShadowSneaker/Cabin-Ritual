using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float Health = 50f;

    public PlayersPoints ThePlayer;

    private void Start()
    {
        ThePlayer = GameObject.Find("Player With Gun").GetComponent<PlayersPoints>();
    }

    public void TakeDamage(float amount)
    {
        Health -= amount;
        if(Health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        ThePlayer.PointsAquired += 100;
        Destroy(gameObject);
    }

    public void PlayerTakeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0f)
        {
            Die();
        }
    }

}

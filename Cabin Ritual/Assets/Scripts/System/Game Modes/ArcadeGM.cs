using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeGM : GameMode
{
    


    /// Properties

    [Header("Spawning")]

    [Tooltip("Allows the zombies to start randomly spawning around the player(s).")]
    [SerializeField]
    private bool AllowSpawning = true;

    [Tooltip("A reference to the zombie prefab types")]
    [SerializeField]
    private GameObject[] ZombieTypes = null;

    [Tooltip("The frequency zombies spawn in the world (In seconds).")]
    [SerializeField]
    private float SpawnRate = 0.5f;


    [Tooltip("The fastest rate zombies can spawn (in seconds).")]
    [SerializeField]
    private float SpawnRateCap = 0.001f;

    [Tooltip("The percentage increment for increasing the zombie spawn rate over time.")]
    [SerializeField]
    private float SpawnMultiplier = 1.0f;

    [Tooltip("The invervals to apply the SpawnMultiplier increment (In seconds).")]
    [SerializeField]
    private float IncreaseRateInverval = 60.0f;

    
    // Represents if the timer to spawn a zombie is running.
    private bool Spawning = false;

    private bool DecrementRate = false;

    private float RatePercent = 100.0f;


    /// Overridables


    // Constatntly loops spawning zombies.
    private void FixedUpdate()
    {
        if (AllowSpawning)
        {
            if (!Spawning)
            {
                StartCoroutine(SpawnTimer());
                Spawning = true;
            }

            if (!DecrementRate)
            {
                StartCoroutine(DecrementTimer());
                DecrementRate = true;
            }
        }
    }



    /// Functions

    
    // The timer to spawn zombies.
    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(Mathf.Lerp(SpawnRateCap, SpawnRate, RatePercent / 100.0f));
        SpawnNearRandomPlayer(Pool.GetRandomKey());
        Spawning = false;
    }


    private IEnumerator DecrementTimer()
    {
        yield return new WaitForSeconds(IncreaseRateInverval);
        RatePercent -= SpawnMultiplier;
        DecrementRate = false;
    }



    public GameObject SpawnZombieRandom()
    {
        // Find a location to spawn the zombie


        return Instantiate(ZombieTypes[Random.Range(0, ZombieTypes.Length)]);
    }


    public GameObject SpawnZombie(Vector3 Location, Quaternion Rotation)
    {
        if (VerifyLocation(Location))
        {
            return Instantiate(ZombieTypes[Random.Range(0, ZombieTypes.Length)], Location, Rotation);
        }
        return null;
    }
}

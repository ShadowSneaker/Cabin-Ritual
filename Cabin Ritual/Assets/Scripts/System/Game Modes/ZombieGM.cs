using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ZombieGM : GameMode
{
    /// Properties

    [Tooltip("A reference to the zombie prefab types")]
    [SerializeField]
    private GameObject[] ZombieTypes = null;
    


    /// Overridables




    /// Functions



    


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

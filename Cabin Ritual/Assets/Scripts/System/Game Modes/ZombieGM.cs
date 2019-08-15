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


    override public void PlayerDown(Controller Player) 
    {
        List<GameObject> Zombies = Pool.GetAllActiveObjects("Zombies");

        if (Zombies.Capacity > 0)
        {
            float ShortestDist = Mathf.Infinity;
            GameObject SelectedZombie = null;

            for (int i = 0; i < Zombies.Capacity; ++i)
            {
                float Dist = Vector3.Distance(Player.transform.position, Zombies[i].transform.position);
                if (Dist < ShortestDist)
                {
                    SelectedZombie = Zombies[i];
                    ShortestDist = Dist;
                }
            }

            // TODO: Make the selected zombie move towards the downed player and drag the player to the crypt.
        }
    }


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

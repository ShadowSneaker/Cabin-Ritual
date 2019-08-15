using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sets and applies all the game rules directing the game in a specified direction.
[RequireComponent(typeof(ObjectPool))]
public class GameMode : MonoBehaviour
{
    private struct PlayerInfo
    {
        public Entity Player;
        public Controller PlayerController;
    }


    [Tooltip("How many players are in the game (1 - 4")]
    [SerializeField]
    private int PlayerCount = 1;

    [Tooltip("Forces the inputted entities to become the players.")]
    [SerializeField]
    private Controller[] StaticPlayers = null;

    [Tooltip("The max distance objects can spawn from a player.")]
    [SerializeField]
    private float MaxSpawnDistance = 200.0f;

    [Tooltip("The maximum amount of times the game will attempt to find a spawning location before quitting.")]
    [SerializeField]
    private int TrySpawnAttempts = 200;

    
    // A reference to all the players in the game.
    private PlayerInfo[] Players;

    // A reference to all the availuable spawn zones.
    private List<SpawnZone> SpawnZones = new List<SpawnZone>();
    

    // A reference to the attached Object Pool component.
    protected ObjectPool Pool;




    /// Overridables



    // Start is called before the first frame update
    void Start()
    {
        Pool = GetComponent<ObjectPool>();
        Players = new PlayerInfo[PlayerCount];

        if (StaticPlayers.Length != 0)
        {
            for (int i = 0; i < StaticPlayers.Length; ++i)
            {
                Players[i].PlayerController = StaticPlayers[i];
                Players[i].Player = StaticPlayers[i].GetComponent<Entity>();
            }
        }
        else
        {
            // Find controllers instead of entities incase there are pre-placed entities in the world.
            Controller[] Controllers = FindObjectsOfType<Controller>();
            for (int i = 0; i < PlayerCount; ++i)
            {
                Players[i].PlayerController = Controllers[i];
                Players[i].Player = Controllers[i].GetComponent<Entity>();
            }
        }
    }


    virtual public void PlayerDown(Controller Player) { }



    
    /// Functions

    
    // Checks a position to see if something can spawn there.
    // Things it checks for are: If the position is not blocked and
    // if the player can see that position (and vision is not blocked).
    // @param Location - The location to check to see if something can spawn there.
    // @return - Returns true if something can spawn there.
    protected bool VerifyLocation(Vector3 Location)
    {
        return true;
    }


    public GameObject Spawn(string Key)
    {
        return Pool.CheckoutObject(Key);
    }


    public GameObject Spawn(string Key, Vector3 Location)
    {
        GameObject Object = Pool.CheckoutObject(Key);
        if (Object)
        {
            Object.transform.position = Location;
        }
        return Object;
    }


    public GameObject Spawn(string Key, Vector3 Location, Quaternion Rotation)
    {
        GameObject Object = Pool.CheckoutObject(Key);
        if (Object)
        {
            Object.transform.position = Location;
            Object.transform.rotation = Rotation;
        }
        return Object;
    }


    public GameObject Spawn(string Key, Vector3 Location, Quaternion Rotation, Vector3 Scale)
    {
        GameObject Object = Pool.CheckoutObject(Key);
        if (Object)
        {
            Object.transform.position = Location;
            Object.transform.rotation = Rotation;
            Object.transform.localScale = Scale;
        }
        return Object;
    }


    public GameObject Spawn(string Key, Transform InTransform)
    {
        GameObject Object = Pool.CheckoutObject(Key);
        if (Object)
        {
            Object.transform.position = InTransform.position;
            Object.transform.rotation = InTransform.rotation;
            Object.transform.localScale = InTransform.localScale;
        }
        return Object;
    }


    // Spawns an object at a spawn zone near the inputted player.
    // @param Key - The object type that should be spawned.
    // @param Player - The player to spawn near.
    public GameObject SpawnNearPlayer(string Key, Controller Player)
    {
        if (Player && SpawnZones.Count > 0)
        {
            for (int i = 0; i < TrySpawnAttempts; ++i)
            {
                SpawnZone Zone = SpawnZones[Random.Range(0, SpawnZones.Count)];
                if (Vector3.Distance(Zone.transform.position, Player.transform.position) <= MaxSpawnDistance)
                {
                    return Spawn(Key, Zone.transform);
                }
            }
        }

        return null;
    }


    // Spawns an object at a spawn zone near the inputted player.
    // @param Key - The object type that should be spawned.
    // @param Player - The player to spawn near.
    public GameObject SpawnNearPlayer(string Key, int PlayerIndex)
    {
        return SpawnNearPlayer(Key, Players[PlayerIndex].PlayerController);
    }


    // Spawns an object at a spawn zone near a random player.
    // @param Key - The object type that should be spawned.
    public GameObject SpawnNearRandomPlayer(string Key)
    {
        return SpawnNearPlayer(Key, Players[Random.Range(0, Players.Length)].PlayerController);
    }


    // Despawns an object from the game.
    // @param Key - The object type that should be spawned.
    // @param Object - The object to be despawned.
    public void Despawn(string Key, GameObject Object)
    {
        Pool.ReturnObject(Key, Object);
    }


    public void AddSpawnZone(SpawnZone Zone)
    {
        SpawnZones.Add(Zone);
    }


    public void RemoveSpawnZone(SpawnZone Zone)
    {
        SpawnZones.Remove(Zone);
    }


    // Returns the amount of players there are in the game.
    public int GetPlayerCount()
    {
        return Players.Length;
    }


    public float GetMaxSpawnDistance
    {
        get
        {
            return MaxSpawnDistance;
        }
    }

    public Controller GetPlayerController(int Index)
    {
        return Players[Index].PlayerController;
    }


    public Entity GetPlayer(int Index)
    {
        return Players[Index].Player;
    }
}

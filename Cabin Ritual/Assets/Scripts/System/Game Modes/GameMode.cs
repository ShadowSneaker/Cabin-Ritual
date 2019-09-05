using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Sets and applies all the game rules directing the game in a specified direction.
[RequireComponent(typeof(ObjectPool))]
public class GameMode : MonoBehaviour
{
    public enum EOption
    {
        Yes,
        No,
        Both
    }

    [System.Serializable]
    public struct SSpawnParams
    {
        [Tooltip("Should the entity be spawned near the player.")]
        public bool SpawnNearPlayer;

        [Tooltip("Should the entity only be spawned at a valid spawner.")]
        public EOption SpawnAtValid;

        [Tooltip("Forces this entity to be spawned (leave empty for a random entity type).")]
        public string ForceType;

        [Tooltip("Forces the entity to be spawned near a specified player using the player index (Set to -1 for a random player).")]
        public int ForceByPlayer;

        [Tooltip("Forces the entity to be spawned at a specific transform (Set to null for a random spawner.")]
        public Transform ForceTransform;
    }
    

    private struct PlayerInfo
    {
        public Entity Player;
        public Controller PlayerController;
    }

    

    [Header("Force Spawning")]



    [Tooltip("Forcse a entity to be spawned in the world.")]
    [SerializeField]
    private bool ForceSpawn = false;

    [Tooltip("The spawn parameters for force spawning an entity.")]
    [SerializeField]
    private SSpawnParams SpawnParams = new SSpawnParams();


    [Header("General")]


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


    private void FixedUpdate()
    {
        CheckForceSpawn();
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


    public GameObject SpawnAwayPlayer(string Key, Controller Player)
    {
        if (Player && SpawnZones.Count > 0)
        {
            for (int i = 0; i < TrySpawnAttempts; ++i)
            {
                SpawnZone Zone = SpawnZones[Random.Range(0, SpawnZones.Count)];
                if (Vector3.Distance(Zone.transform.position, Player.transform.position) >= MaxSpawnDistance)
                {
                    return Spawn(Key, Zone.transform);
                }
            }
        }

        return null;
    }


    public GameObject SpawnAwayPlayer(string Key, int PlayerIndex)
    {
        return SpawnAwayPlayer(Key, Players[PlayerIndex].PlayerController);
    }


    public GameObject SpawnAwayRandomPlayer(string Key)
    {
        return SpawnAwayPlayer(Key, Players[Random.Range(0, Players.Length)].PlayerController);
    }


    private void CheckForceSpawn()
    {
        if (ForceSpawn)
        {
            ForceSpawn = false;

            string EntityKey = ((SpawnParams.ForceType != "") ? SpawnParams.ForceType : Pool.GetRandomKey());
            int PlayerIndex = ((SpawnParams.ForceByPlayer != -1) ? SpawnParams.ForceByPlayer : Random.Range(0, PlayerCount));

            //Transform SpawnTransform = ((SpawnParams.ForceTransform != null) ? SpawnParams.ForceTransform : SpawnZones[Random.Range(0, SpawnZones.Count)].transform);

            Transform SpawnTransform = null;

            for (int i = 0; i < TrySpawnAttempts; ++i)
            {
                SpawnZone Zone = SpawnZones[Random.Range(0, SpawnZones.Count)];

                if (SpawnParams.SpawnNearPlayer)
                {
                    if (Vector3.Distance(Zone.transform.position, GetPlayer(PlayerIndex).transform.position) <= MaxSpawnDistance)
                    {
                        SpawnTransform = Zone.transform;
                        break;
                    }
                }
                else
                {
                    if (Vector3.Distance(Zone.transform.position, GetPlayer(PlayerIndex).transform.position) >= MaxSpawnDistance)
                    {
                        SpawnTransform = Zone.transform;
                        break;
                    }
                }
            }

            if (SpawnTransform != null)
            {
                Spawn(EntityKey, SpawnTransform);
            }
        }
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

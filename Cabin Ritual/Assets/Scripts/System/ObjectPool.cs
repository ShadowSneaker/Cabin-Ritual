using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// An object that handles spawning and despawning objects in the game.
public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public struct ObjectType
    {

        // The reference of the created object.
        private GameObject Instance;


        public GameObject GetInstance
        {
            get
            {
                return Instance;
            }
        }


        public void SetInstance(GameObject Object)
        {
            Instance = Object;
        }
    }


    [System.Serializable]
    public struct DisplayObject
    {
        [Tooltip("The key used to access this object type")]
        public string Key;

        [Tooltip("A reference to the object prefab.")]
        public GameObject Prefab;

        [Tooltip("How many of this object should be created")]
        public int InstanceCount;
    }



    /// Properties

    [Tooltip("Should the count of objects be scaled up to how many players there are?")]
    [SerializeField]
    private bool ShouldScaleSpawns = true;

    [Tooltip("The multiplier on how many objects should be created per player.")]
    [SerializeField]
    private float ObjectMultiplier = 1.0f;

    [Tooltip("A list of objects that should be created and how much should be created.")]
    [SerializeField]
    private DisplayObject[] CreateObjects = new DisplayObject[1];

   //The types of objects and amount should be spawned (then disabled).
    private Dictionary<string, ObjectType[]> ObjectTypes = new Dictionary<string, ObjectType[]>();


    /// Overridables


    private void Start()
    {
        // Spawn all objects

        GameMode GM = FindObjectOfType<GameMode>();

        for (int i = 0; i < CreateObjects.Length; ++i)
        {
            int SpawnCount = Mathf.RoundToInt(CreateObjects[i].InstanceCount * ObjectMultiplier * ((GM) ? (ShouldScaleSpawns) ? GM.GetPlayerCount() : 1 : 1));

            ObjectTypes.Add(CreateObjects[i].Key, new ObjectType[SpawnCount]);

            for (int j = 0; j < SpawnCount; j++)
            {
                ObjectTypes[CreateObjects[i].Key][j].SetInstance(Instantiate(CreateObjects[i].Prefab));
                ObjectTypes[CreateObjects[i].Key][j].GetInstance.SetActive(false);
            }
        }
    }


    /// Functions


    public GameObject CheckoutObject(string Key)
    {
        if (ObjectTypes.ContainsKey(Key))
        {
            for (int i = 0; i < ObjectTypes[Key].Length; ++i)
            {
                if (!ObjectTypes[Key][i].GetInstance.activeSelf)
                {
                    ObjectTypes[Key][i].GetInstance.SetActive(true);
                    return ObjectTypes[Key][i].GetInstance;
                }
            }
        }
        else
        {
            Debug.LogWarning("Warning: Inputted key does not exist.");
        }
        

        return null;
    }


   
    public void ReturnObject(string Key, GameObject Object)
    {
        if (ObjectTypes.ContainsKey(Key))
        {
            for (int i = 0; i < ObjectTypes[Key].Length; ++i)
            {
                if (ObjectTypes[Key][i].GetInstance == Object)
                {
                    ObjectTypes[Key][i].GetInstance.SetActive(false);
                }
            }

            Debug.LogWarning("Warning: Inputted object does not exist in inputted key.");
        }
        else
        {
            Debug.LogWarning("Warning: Inputted key does not exist.");
        }
    }


    // Returns a reference of all the objects that are checked out of a specified type.
    // @param Key - The object type to retreieve.
    // @return - A list of all active objects.
    public List<GameObject> GetAllActiveObjects(string Key)
    {
        List<GameObject> FoundObjects = new List<GameObject>();

        if (ObjectTypes.ContainsKey(Key))
        {
            for (int i = 0; i < ObjectTypes[Key].Length; ++i)
            {
                if (ObjectTypes[Key][i].GetInstance.activeSelf)
                {
                    FoundObjects.Add(ObjectTypes[Key][i].GetInstance);
                }
            }
        }
        return FoundObjects;
    }


    public string GetRandomKey()
    {
        return new List<string>(ObjectTypes.Keys)[Random.Range(0, ObjectTypes.Count)];
    }


    public List<string> GetAllKeys()
    {
        return new List<string>(ObjectTypes.Keys);
    }
}

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

    [Tooltip("A list of object that should be created and how much should be created.")]
    [SerializeField]
    private DisplayObject[] CreateObjects = new DisplayObject[1];

   //The types of objects and amount should be spawned (then disabled).
    private Dictionary<string, ObjectType[]> ObjectTypes = new Dictionary<string, ObjectType[]>();


    /// Overridables


    private void Start()
    {
        // Spawn all objects

        for (int i = 0; i < CreateObjects.Length; ++i)
        {
            ObjectTypes.Add(CreateObjects[i].Key, new ObjectType[CreateObjects[i].InstanceCount]);

            for (int j = 0; j < CreateObjects[i].InstanceCount; j++)
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject RitualPrefabRat;
    //public GameObject RitualPrefabPlant;
    //public GameObject RitualPrefabKnife;
    //public GameObject RitualPrefabBone;
    //public GameObject RitualPrefabMeat;

    public Vector3 Centre;
    public Vector3 Size;


    // Start is called before the first frame update
    void Start()
    {
        SpawnRitualItemRat();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Q))
        // SpawnRitualItemRat();
    }

    public void SpawnRitualItemRat()
    {
        Vector3 pos = Centre + new Vector3(Random.Range(-Size.x / 2, Size.x / 2), Random.Range(-Size.y / 2, Size.y / 2), Random.Range(-Size.z / 2, Size.z / 2));

        GameObject Temp = Instantiate(RitualPrefabRat, pos, Quaternion.identity);

        Temp.transform.parent = GameObject.Find("ItemPool").transform;

        //Instantiate(RitualPrefabPlant, pos, Quaternion.identity);
        //Instantiate(RitualPrefabKnife, pos, Quaternion.identity);
        //Instantiate(RitualPrefabBone, pos, Quaternion.identity);
        //Instantiate(RitualPrefabMeat, pos, Quaternion.identity);

    }

    public void OnDrawSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(Centre, Size);
    }
}

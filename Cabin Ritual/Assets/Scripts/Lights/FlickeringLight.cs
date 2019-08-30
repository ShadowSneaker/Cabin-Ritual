using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    Light Lights;
    public float MinWaitTime;
    public float MaxWaitTime;

    void Start()
    {
        Lights = GetComponent<Light>();
        StartCoroutine(Flashing());        
    }

    IEnumerator Flashing()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(MinWaitTime, MaxWaitTime));
            Lights.enabled = !Lights.enabled;
        }
    }

   
}

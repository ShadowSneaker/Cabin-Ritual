using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOn : MonoBehaviour
{
    public GameObject Light;
    private bool LightActivation;

    void start()
    {
        LightActivation = false;
    }
    public void TurnOnLight()
    {
        Controller temp = FindObjectOfType<Controller>();
        if (temp.ReturnLookingAt())
        {

            Light.SetActive(true);
        }        
    }   
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLightOn : MonoBehaviour
{
    public GameObject Light;
    public GameObject Light2;
    public GameObject Light3;
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
            Light2.SetActive(true);
            Light3.SetActive(true);
        }
    }

}
